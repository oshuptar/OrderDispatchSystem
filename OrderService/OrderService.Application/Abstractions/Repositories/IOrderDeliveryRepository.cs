using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IOrderDeliveryRepository
{
    Task CreateOrderDeliveryAsync(OrderDelivery orderDelivery, CancellationToken cancellationToken);
}