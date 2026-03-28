using System.Text.Json;
using Auth.Abstractions.Persistence;
using Auth.Infrastructure.KafkaTopics;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.EventStreaming.IntegrationEvents;
using OrderService.Application.Features.Admin.OrderFinalizeVerification.Contracts;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Models.Outbox;
using OrderService.Domain.Enums;
using OrderService.Domain.Models.Enums;

namespace OrderService.Application.Features.Admin.OrderFinalizeVerification;

public class OrderFinalizeVerificationCommandHandler(
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order> orderGetByIdQueryHandler,
    ICommandHandler<OrderUpdateRequest> orderUpdateCommandHandler,
    IOutboxMessageRepository outboxMessageRepository,
    IUnitOfWork unitOfWork,
    ILogger<OrderFinalizeVerificationCommandHandler> logger
        ) : ICommandHandler<OrderFinalizeVerificationRequest>
{
    public async Task HandleCommandAsync(OrderFinalizeVerificationRequest command, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            logger.LogInformation("[{dateTime}]: Finalizing verification for {orderId}", DateTime.UtcNow, command.Id);
            Domain.Models.Order order = await  orderGetByIdQueryHandler.HandleQueryAsync(new OrderGetByIdRequest(command.Id), cancellationToken);
            if(order.OrderStatus >= OrderStatus.Verified)
                throw new InvalidOperationException($"Order {order.Id} is already verified");
            await orderUpdateCommandHandler.HandleCommandAsync(new OrderUpdateRequest(command.Id, OrderStatus.Verified), cancellationToken);

            // Outbox implementation:
            OrderVerifiedIntegrationEvent integrationEvent = new OrderVerifiedIntegrationEvent(
                order.Id,
                order.UserId,
                order.Volume,
                order.Weight,
                order.RequestedDeliveryDateTime,
                DateTime.UtcNow
            );
            await outboxMessageRepository.CreateAsync(
                new OutboxMessageCreateModel(
                    Guid.NewGuid(),
                    nameof(EventTopic.Order),
                    integrationEvent.OrderId.ToString(),
                    JsonSerializer.Serialize(integrationEvent),
                    OrderEventType.OrderVerified
                ), cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation("[{dateTime}]: Finalizing verification for {orderId} completed", DateTime.UtcNow, command.Id);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[{dateTime}]: Canceled finalizing verification for {orderId}", DateTime.UtcNow, command.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("[{dateTime}]: Finalizing verification for {orderId} failed", DateTime.UtcNow, command.Id);
            throw;
        }
    }
}