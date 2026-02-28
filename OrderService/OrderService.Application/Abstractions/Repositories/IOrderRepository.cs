using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order order, CancellationToken cancellationToken);
}