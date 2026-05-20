using OrderService.Application.Models.OrderDelivery;
using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IOrderDeliveryRepository
{
    Task CreateAsync(OrderDelivery orderDelivery, CancellationToken cancellationToken);
    Task UpdateAsync(OrderDeliveryUpdateRequestModel request, CancellationToken cancellationToken);
    Task<OrderDelivery?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
}