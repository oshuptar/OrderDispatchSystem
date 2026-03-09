using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.OrderDelivery.Create.Contracts;
using OrderService.Application.Mappers;

namespace OrderService.Application.Features.OrderDelivery.Create;

public class OrderDeliveryCreateCommandHandler(
    IOrderDeliveryRepository orderDeliveryRepository
    ) : ICommandHandler<OrderDeliveryCreateRequest, OrderDeliveryCreateResponse>
{
    public async Task<OrderDeliveryCreateResponse> HandleCommandAsync(OrderDeliveryCreateRequest command, CancellationToken cancellationToken)
    {
        if(command.ScheduledDeliveryDateTime > DateTime.UtcNow)
            throw new InvalidOperationException("The scheduled order delivery date cannot be in the future");
        
        Domain.Models.OrderDelivery entity = command.ToDomainModel();
        await orderDeliveryRepository.CreateOrderDeliveryAsync(entity, cancellationToken);
        return new OrderDeliveryCreateResponse(entity.Id);
    }
}