using Auth.Abstractions.Persistence;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;
using OrderService.Application.Mappers;
using OrderService.Application.Models;
using OrderService.Domain.Models;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.OrderPlatform.OrderCreate;

public class ClientOrderCreateCommandHandler (
    IOrderDeliveryRepository orderDeliveryRepository,
    IOrderRepository orderRepository,
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork,
    ILogger<ClientOrderCreateCommandHandler> logger
    ): ICommandHandler<ClientCreateOrderRequest, ClientCreateOrderResponse>
{
    // TODO: create commandHandlers per CRUD operation and then reuse instead of calling repository
    public async Task<ClientCreateOrderResponse> HandleCommandAsync(
        ClientCreateOrderRequest command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[{dateTime}]: Creating order", DateTime.Now);
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if(DateTime.Now > command.ScheduledOrderDateTime)
                throw new InvalidOperationException("The scheduled order date cannot be in the future");
            
            var deliveryAddress = command.DeliveryAddressRequest.ToDomainModel();
            var normalisedDeliveryAddress = deliveryAddress.CheckAddressValidity();
            AddressSearchRequestModel model = new AddressSearchRequestModel(
                normalisedDeliveryAddress.Country,
                normalisedDeliveryAddress.Region,
                normalisedDeliveryAddress.City,
                normalisedDeliveryAddress.Street,
                normalisedDeliveryAddress.Apartment,
                normalisedDeliveryAddress.PostalCode,
                normalisedDeliveryAddress.Longitude,
                normalisedDeliveryAddress.Latitude,
                Page: 0,
                Size: 1
            );
            var resolvedAddress = (await addressRepository.GetAddressesAsync(model, cancellationToken)).FirstOrDefault();
            if (resolvedAddress is null)
            {
                await addressRepository.CreateAddressAsync(normalisedDeliveryAddress, cancellationToken);
                resolvedAddress = normalisedDeliveryAddress;
            }

            Order order = command.ToDomainModel();
            await orderRepository.CreateOrderAsync(order, cancellationToken);

            OrderDelivery orderDelivery = new OrderDelivery()
            {
                DestinationAddressId = resolvedAddress.Id,
                DestinationAddress = resolvedAddress,
                ScheduledDeliveryDateTime = order.ScheduledOrderDateTime,
                OrderId = order.Id,
                Order = order
            };
            await orderDeliveryRepository.CreateOrderDeliveryAsync(orderDelivery, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return new ClientCreateOrderResponse(order.Id, orderDelivery.Id);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[{dateTime}]: Creating order cancelled", DateTime.Now);
            await unitOfWork.RollbackTransactionAsync(transaction, CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("[{dateTime}]: Creating order failed: {message}", DateTime.Now, ex.Message);
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }
}