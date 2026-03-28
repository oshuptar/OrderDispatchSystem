using System.Text.Json;
using Auth.Abstractions.Persistence;
using Auth.Infrastructure.KafkaTopics;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.EventStreaming.IntegrationEvents;
using OrderService.Application.Features.Admin.OrderAssignProductionPlantCommandHandler.Contracts;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Features.OrderDelivery.Update.Contracts;
using OrderService.Application.Features.ProductionPlant.Get.Contracts;
using OrderService.Application.Models.Outbox;
using OrderService.Domain.Enums;
using OrderService.Domain.Models.Enums;

namespace OrderService.Application.Features.Admin.OrderAssignProductionPlantCommandHandler;

public class OrderAssignProductionPlantCommandHandler(
    ICommandHandler<OrderUpdateRequest> orderUpdateCommandHandler,
    ICommandHandler<OrderDeliveryUpdateRequest> orderDeliveryUpdateCommandHandler,
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order> orderGetByIdQueryHandler,
    IQueryHandler<ProductionPlantGetByIdRequest, Domain.Models.ProductionPlant> productionPlantGetByIdQueryHandler,
    IUnitOfWork unitOfWork,
    ILogger<OrderAssignProductionPlantCommandHandler> logger,
    IOutboxMessageRepository outboxMessageRepository
        ) : ICommandHandler<OrderAssignProductionPlantRequest>
{
    public async Task HandleCommandAsync(OrderAssignProductionPlantRequest command, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            logger.LogInformation(
                "[{dateTime}]: Assigning production plant with id: {plantId} to order with id: {orderId}",
                DateTime.UtcNow, command.ProductionPlantId, command.OrderId);
            Domain.Models.Order order = await orderGetByIdQueryHandler.HandleQueryAsync(new OrderGetByIdRequest(command.OrderId), cancellationToken);
            
            if(order.OrderStatus <= OrderStatus.Verified)
                throw new InvalidOperationException($"Order {order.Id} is not yet verified");
            
            // TO change the value of production plant id expose the update order functionality
            if(order.ProductionPlantId.HasValue)
                throw new InvalidOperationException($"Order {order.Id} already has a production plant assigned");

            Domain.Models.ProductionPlant plant = 
                await productionPlantGetByIdQueryHandler.HandleQueryAsync(new ProductionPlantGetByIdRequest(command.ProductionPlantId), cancellationToken);
                
            await orderUpdateCommandHandler.HandleCommandAsync(
                new OrderUpdateRequest(
                    command.OrderId,
                    ProductionPlantId: command.ProductionPlantId), cancellationToken
                );
            
            await orderUpdateCommandHandler.HandleCommandAsync(new OrderUpdateRequest(
                command.OrderId,
                ProductionPlantId: command.ProductionPlantId), cancellationToken);
            await orderDeliveryUpdateCommandHandler.HandleCommandAsync(new OrderDeliveryUpdateRequest(
                command.OrderId,
                SourceAddressId: plant.AddressId
            ), cancellationToken);
            
            // Outbox implementation:
            OrderProductionPlantAssignedIntegrationEvent integrationEvent = 
                new OrderProductionPlantAssignedIntegrationEvent(order.Id, plant.Id, DateTime.UtcNow);
            await outboxMessageRepository.CreateAsync(new OutboxMessageCreateModel(
                Guid.NewGuid(),
                nameof(EventTopic.Order),
                integrationEvent.OrderId.ToString(),
                JsonSerializer.Serialize(integrationEvent),
                OrderEventType.OrderProductionPlantAssigned), cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation(
                "[{dateTime}]: Assigned production plant with id: {plantId} to order with id: {orderId}",
                DateTime.UtcNow, command.ProductionPlantId, command.OrderId);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation(
                "[{dateTime}]: Assigning production plant with id: {plantId} to order with id: {orderId} is cancelled",
                DateTime.UtcNow, command.ProductionPlantId, command.OrderId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[{dateTime}]: Error occurred while assigning production plant with id: {plantId} to order with id: {orderId}",
                DateTime.UtcNow, command.ProductionPlantId, command.OrderId);
            throw;
        }
    }
}