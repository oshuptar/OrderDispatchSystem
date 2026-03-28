using System.Text.Json;
using Auth.Abstractions.Persistence;
using Auth.Exceptions;
using Auth.Infrastructure.KafkaTopics;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Context;
using OrderService.Application.EventStreaming.IntegrationEvents;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Features.OrderDelivery.Update.Contracts;
using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;
using OrderService.Application.Models.Outbox;
using OrderService.Domain.Enums;
using OrderService.Domain.Models.Enums;

namespace OrderService.Application.Features.OrderPlatform.UpdateOrder;

public class ClientOrderUpdateCommandHandler(
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order>  orderGetByIdQueryHandler,
    ICommandHandler<OrderUpdateRequest> orderUpdateCommandHandler,
    ICommandHandler<OrderDeliveryUpdateRequest> orderDeliveryUpdateCommandHandler,
    IUnitOfWork unitOfWork,
    ILogger<ClientOrderUpdateCommandHandler> logger,
    IOutboxMessageRepository outboxMessageRepository,
    UserContext userContext
    ) : ICommandHandler<ClientOrderUpdateRequest>
{
    public async Task HandleCommandAsync(ClientOrderUpdateRequest command, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Domain.Models.Order order = await orderGetByIdQueryHandler.HandleQueryAsync(
                new OrderGetByIdRequest(command.OrderId), cancellationToken);

            if (order.OrderStatus >= OrderStatus.Verifying)
                throw new BadRequestException("Order is verifying. No Changes allowed");
    
            await orderUpdateCommandHandler.HandleCommandAsync(
                new OrderUpdateRequest(command.OrderId,
                    Volume: command.Volume,
                    Weight: command.Weight,
                    RequestedDeliveryDateTime: command.RequestedDeliveryDateTime)
                , cancellationToken);

            if (command.DestionationAddressUpdateRequest is not null || command.RequestedDeliveryDateTime is not null)
            {
                await orderDeliveryUpdateCommandHandler.HandleCommandAsync(
                    new OrderDeliveryUpdateRequest(order.Id,
                         command.RequestedDeliveryDateTime,
                         command.DestionationAddressUpdateRequest), cancellationToken);
            }
            
            // Outbox implementation:
            ClientOrderUpdatedIntegrationEvent integrationEvent = 
                new ClientOrderUpdatedIntegrationEvent(
                command.OrderId,
                userContext.User!.Id,
                DateTime.UtcNow);
            await outboxMessageRepository.CreateAsync(
                new OutboxMessageCreateModel(Id: Guid.NewGuid(),
                    Topic: nameof(EventTopic.Order),
                    Key: command.OrderId.ToString(),
                    Payload: JsonSerializer.Serialize(integrationEvent),
                    EventType: OrderEventType.ClientOrderUpdated),
                cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation("[{dateTime}]: Client - Order updated successfully", DateTime.UtcNow);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[{dateTime}]: Client - Updating order cancelled", DateTime.UtcNow);
            throw;
        }  
        catch (Exception ex)
        {
            logger.LogError("[{dateTime}]: Client - Updating order failed: {message}", DateTime.UtcNow, ex.Message);
            throw;
        }
    }
}