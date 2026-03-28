using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Models;
using OrderService.Application.Models.Order;

namespace OrderService.Application.Features.Order.Update;

// TODO: add logging
public class OrderUpdateCommandHandler(
    IProductionPlantRepository productionPlantRepository,
    IOrderRepository orderRepository
    ) : ICommandHandler<OrderUpdateRequest>
{
    public async Task HandleCommandAsync(OrderUpdateRequest command, CancellationToken cancellationToken)
    {
        bool exists = await orderRepository.ExistsByIdAsync(command.OrderId, cancellationToken);
        if(!exists)
            throw new NotFoundException($"Order with  id {command.OrderId} not found");
        if(command.RequestedDeliveryDateTime.HasValue && command.RequestedDeliveryDateTime.Value > DateTime.UtcNow)
            throw new InvalidOperationException("The scheduled order date cannot be in the future");
        if(command.Volume <= 0)
            throw new InvalidOperationException("Volume cannot be zero or negative");
        if(command.Weight <= 0)
            throw new InvalidOperationException("Weight cannot be zero or negative");
        if(command.ProductionPlantId.HasValue && !await productionPlantRepository.ExistsByIdAsync(command.ProductionPlantId.Value, cancellationToken))
            throw new NotFoundException($"Production plant with id {command.ProductionPlantId} not found");

        await orderRepository.UpdateAsync(
            new OrderUpdateRequestModel(command.OrderId,
            command.OrderStatus, command.Volume, command.Weight, command.RequestedDeliveryDateTime,
            command.ProductionPlantId), cancellationToken);
    }
}