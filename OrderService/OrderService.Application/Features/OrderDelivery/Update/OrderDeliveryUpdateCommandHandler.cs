using Auth.Mediator.Interfaces;
using OrderService.Application.Features.OrderDelivery.Update.Contracts;

namespace OrderService.Application.Features.OrderDelivery.Update;

public class OrderDeliveryUpdateCommandHandler : ICommandHandler<OrderDeliveryUpdateRequest>
{
    public Task HandleCommandAsync(OrderDeliveryUpdateRequest command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}