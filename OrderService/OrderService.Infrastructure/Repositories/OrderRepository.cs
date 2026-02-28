using OrderService.Application.Abstractions.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository(OrderDbContext orderDbContext) : IOrderRepository
{
    public async Task CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        OrderEntity entity = order.ToEntity();
        await orderDbContext.AddAsync(entity, cancellationToken);
    }
}