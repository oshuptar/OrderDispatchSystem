using Auth.Abstractions.Persistence;
using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.EventStreaming;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Features.OrderDelivery.Update.Contracts;
using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.OrderPlatform.UpdateOrder;

public class ClientOrderUpdateCommandHandler(
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order>  orderGetByIdQueryHandler,
    ICommandHandler<OrderUpdateRequest> orderUpdateCommandHandler,
    ICommandHandler<OrderDeliveryUpdateRequest> orderDeliveryUpdateCommandHandler,
    IUnitOfWork unitOfWork,
    ILogger<ClientOrderUpdateCommandHandler> logger,
    IEventProducer eventProducer
    ) : ICommandHandler<ClientOrderUpdateRequest>
{
    public async Task HandleCommandAsync(ClientOrderUpdateRequest command, CancellationToken cancellationToken)
    {
        var transaction = await  unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Domain.Models.Order order = await orderGetByIdQueryHandler.HandleQueryAsync(
                new OrderGetByIdRequest(command.OrderId), cancellationToken);

            if (order.OrderStatus >= OrderStatus.Verifying)
                throw new BadRequestException("Order is verifying. No Changes allowed");
    
            await orderUpdateCommandHandler.HandleCommandAsync(
                new OrderUpdateRequest(command.OrderId,
                    command.OrderStatus,
                    command.Volume,
                    command.Weight,
                    command.RequestedDeliveryDateTime)
                , cancellationToken);

            if (command.DestionationAddressUpdateRequest is not null)
            {
                await orderDeliveryUpdateCommandHandler.HandleCommandAsync(
                    new OrderDeliveryUpdateRequest(order.Id,
                        command.DestionationAddressUpdateRequest), cancellationToken);
            }
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