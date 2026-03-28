using System.Text.Json;
using Auth.Abstractions.Persistence;
using Auth.Infrastructure.KafkaTopics;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Context;
using OrderService.Application.EventStreaming.IntegrationEvents;
using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Features.Order.Create.Contracts;
using OrderService.Application.Features.OrderPlatform.PostOrder.Contracts;
using OrderService.Application.Mappers;
using OrderService.Application.Models.Address;
using OrderService.Application.Models.Outbox;
using OrderService.Domain.Models.Enums;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.OrderPlatform.PostOrder;

public class ClientOrderCreateCommandHandler (
    UserContext userContext,
    IAddressRepository addressRepository,
    ICommandHandler<AddressCreateRequest, AddressCreateResponse> addressCreateCommandHandler,
    ICommandHandler<OrderCreateRequest, OrderCreateResponse> orderCreateCommandHandler,
    IUnitOfWork unitOfWork,
    ILogger<ClientOrderCreateCommandHandler> logger,
    IOutboxMessageRepository outboxMessageRepository
    ): ICommandHandler<ClientCreateOrderRequest, ClientCreateOrderResponse>
{
    public async Task<ClientCreateOrderResponse> HandleCommandAsync(
        ClientCreateOrderRequest command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[{dateTime}]: Creating order", DateTime.UtcNow);
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
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
                normalisedDeliveryAddress.Latitude, Page: 0, Size: 1);
            Guid? resolvedAddressId = (await addressRepository.GetAsync(model, cancellationToken)).FirstOrDefault()?.Id;
            if (resolvedAddressId is null)  
            {
                AddressCreateResponse addressCreateResponse = await addressCreateCommandHandler.HandleCommandAsync(new AddressCreateRequest(
                    normalisedDeliveryAddress.Country,
                    normalisedDeliveryAddress.Region,
                    normalisedDeliveryAddress.City,
                    normalisedDeliveryAddress.Street,
                    normalisedDeliveryAddress.Apartment,
                    normalisedDeliveryAddress.PostalCode,
                    normalisedDeliveryAddress.Longitude,
                    normalisedDeliveryAddress.Latitude), cancellationToken);
                resolvedAddressId = addressCreateResponse.Id;
            }
            OrderCreateResponse res = await orderCreateCommandHandler.HandleCommandAsync(new OrderCreateRequest(
                userContext.User!.Id,
                command.Volume,
                command.Weight,
                command.ScheduledOrderDateTime,
                resolvedAddressId.Value), cancellationToken);
            
            // Outbox implementation:
            ClientOrderCreatedIntegrationEvent integrationEvent =
                new ClientOrderCreatedIntegrationEvent(
                    OrderId: res.Id,
                    UserId: userContext.User!.Id,
                    DateTime.UtcNow);
            await outboxMessageRepository.CreateAsync(
                new OutboxMessageCreateModel(
                    Id: Guid.NewGuid(),
                    Topic: nameof(EventTopic.Order),
                    Key: res.Id.ToString(),
                    Payload: JsonSerializer.Serialize(integrationEvent),
                    EventType: OrderEventType.ClientOrderCreated
                ), cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation("[{dateTime}]: Order created successfully", DateTime.UtcNow);
            return new ClientCreateOrderResponse(res.Id);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[{dateTime}]: Creating order cancelled", DateTime.UtcNow);
            await unitOfWork.RollbackTransactionAsync(transaction, CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("[{dateTime}]: Creating order failed: {message}", DateTime.UtcNow, ex.Message);
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }
}
