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
        if(DateTime.UtcNow > command.ScheduledDeliveryDateTime)
            throw new InvalidOperationException("The scheduled order delivery must be in the future");
        
        Domain.Models.OrderDelivery entity = command.ToDomainModel();
        await orderDeliveryRepository.CreateAsync(entity, cancellationToken);
        return new OrderDeliveryCreateResponse(entity.Id);
    }
}