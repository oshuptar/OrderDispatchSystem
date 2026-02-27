using Auth.Abstractions.Persistence;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;
using OrderService.Application.Mappers;
using OrderService.Domain.Models;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.OrderPlatform.OrderCreate;

public class ClientOrderCreateCommandHandler (
    IQueryHandler<AddressSearchRequest, AddressSearchResponse> addressSearchRequestHandler,
    IUnitOfWork unitOfWork,
    ILogger<ClientOrderCreateCommandHandler> logger
    ): ICommandHandler<ClientCreateOrderRequestModel, ClientCreateOrderResponseModel>
{
    public async Task<ClientCreateOrderResponseModel> HandleCommandAsync(
        ClientCreateOrderRequestModel command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[{dateTime}]: Creating order", DateTime.Now);
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if(DateTime.Now > command.ScheduledOrderDateTime)
                throw new InvalidOperationException("The scheduled order date cannot be in the future");
            
            var deliveryAddress = command.DeliveryAddress.ToDomainModel();
            var normalisedDeliveryAddress = deliveryAddress.CheckAddressValidity();
            AddressSearchResponse res = await addressSearchRequestHandler.HandleQueryAsync(new AddressSearchRequest(
                normalisedDeliveryAddress.Country,
                normalisedDeliveryAddress.Region,
                normalisedDeliveryAddress.City,
                normalisedDeliveryAddress.Street,
                normalisedDeliveryAddress.Apartment,
                normalisedDeliveryAddress.PostalCode,
                normalisedDeliveryAddress.Longitude,
                normalisedDeliveryAddress.Latitude
            ), cancellationToken);

            var resolvedAddress = res.Requests.FirstOrDefault();
            if (resolvedAddress == null || res.TotalCount == 0)
            {
                // TODO: save a new address to the db and assign to resolvedAddress var
            }
            
            // TODO: create Order and store in DB
            Order order = command.ToDomainModel();

            OrderDelivery orderDelivery = new OrderDelivery()
            {
                DeliveryAddressId = normalisedDeliveryAddress.Id,
                DeliveryAddress = normalisedDeliveryAddress,
                ScheduledDeliveryDateTime = order.ScheduledOrderDateTime,
                OrderId = order.Id,
                Order = order
            };
            
            return new ClientCreateOrderResponseModel(order.Id, orderDelivery.Id);
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