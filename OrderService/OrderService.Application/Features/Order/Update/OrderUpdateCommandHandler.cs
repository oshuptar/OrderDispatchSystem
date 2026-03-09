using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Features.Order.Update.Contracts;

namespace OrderService.Application.Features.Order.Update;

// TODO: add logging
public class OrderUpdateCommandHandler(
    IQueryHandler<OrderGetByIdRequest, Domain.Models.Order> orderGetByIdQueryHandler,
    IProductionPlantRepository productionPlantRepository,
    IOrderRepository orderRepository
    ) : ICommandHandler<OrderUpdateRequest>
{
    public async Task HandleCommandAsync(OrderUpdateRequest command, CancellationToken cancellationToken)
    {
        Domain.Models.Order order = await orderGetByIdQueryHandler.HandleQueryAsync(new OrderGetByIdRequest(command.OrderId), cancellationToken);
        if(command.RequestedDeliveryDateTime.HasValue && command.RequestedDeliveryDateTime.Value > DateTime.UtcNow)
            throw new InvalidOperationException("The scheduled order date cannot be in the future");
        if(command.Volume <= 0)
            throw new InvalidOperationException("Volume cannot be zero or negative");
        if(command.Weight <= 0)
            throw new InvalidOperationException("Weight cannot be zero or negative");
        if(command.ProductionPlantId.HasValue && !await productionPlantRepository.ExistsByIdAsync(command.ProductionPlantId.Value, cancellationToken))
            throw new NotFoundException($"Production plant with id {command.ProductionPlantId} not found");
        
        order.OrderStatus = command.OrderStatus ?? order.OrderStatus;
        order.ProductionPlantId = command.ProductionPlantId;
        order.Volume = command.Volume ?? order.Volume;
        order.Weight = command.Weight ?? order.Weight;
        order.RequestedDeliveryDateTime = command.RequestedDeliveryDateTime ?? order.RequestedDeliveryDateTime;
    }
}