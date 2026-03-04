using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Create.Contracts;
using OrderService.Application.Features.OrderDelivery.Create.Contracts;
using OrderService.Application.Mappers;

namespace OrderService.Application.Features.Order.Create;

// TODO: add logging
public class OrderCreateCommandHandler(
    IOrderRepository orderRepository,
    ICommandHandler<OrderDeliveryCreateRequest, OrderDeliveryCreateResponse> orderDeliveryCreateCommandHandler
    ) : ICommandHandler<OrderCreateRequest, OrderCreateResponse>
{
    public async Task<OrderCreateResponse> HandleCommandAsync(OrderCreateRequest command, CancellationToken cancellationToken)
    {
        if(DateTime.UtcNow > command.RequestedDeliveryDateTime)
            throw new InvalidOperationException("The scheduled order date cannot be in the future");
        if(command.Volume <= 0)
            throw new InvalidOperationException("Volume cannot be zero or negative");
        if(command.Weight <= 0)
            throw new InvalidOperationException("Weight cannot be zero or negative");
        
        Domain.Models.Order order = command.ToDomainModel();
        await orderRepository.CreateOrderAsync(order, cancellationToken);
        await orderDeliveryCreateCommandHandler.HandleCommandAsync(
            new OrderDeliveryCreateRequest(order.Id,
                order.RequestedDeliveryDateTime,
                command.DestinationAddressId),
            cancellationToken);
        
        return new OrderCreateResponse(order.Id);
    }
}