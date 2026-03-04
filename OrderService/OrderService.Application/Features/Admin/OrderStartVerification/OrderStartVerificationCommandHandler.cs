using Auth.Abstractions.Persistence;
using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Features.Admin.OrderStartVerification.Contracts;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Admin.OrderStartVerification;

public class OrderStartVerificationCommandHandler(
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order> orderGetByIdQueryHandler,
    ICommandHandler<OrderUpdateRequest> orderUpdateCommandHandler,
    IUnitOfWork unitOfWork,
    ILogger<OrderStartVerificationCommandHandler> logger
    ) : ICommandHandler<OrderStartVerificationRequest>
{
    public async Task HandleCommandAsync(OrderStartVerificationRequest command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[{dateTime}]: Starting verification for {orderId}", DateTime.UtcNow, command.Id);
        var transaction = unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Domain.Models.Order order = await orderGetByIdQueryHandler.HandleQueryAsync(new OrderGetByIdRequest(command.Id), cancellationToken);
            if(order.OrderStatus >= OrderStatus.Verifying)
                throw new InvalidOperationException($"Order {order.Id} is currently verified or have been already verified");
            await orderUpdateCommandHandler.HandleCommandAsync(new OrderUpdateRequest(command.Id, OrderStatus.Verifying), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[{dateTime}]: Canceled verification for {orderId}", DateTime.UtcNow, command.Id);
            throw;
        }
        catch (Exception e)
        {
            logger.LogError("[{dateTime}]: Starting verification for {orderId} failed", DateTime.UtcNow, command.Id);
            throw;
        }
    }
}