using OrderService.Application.Abstractions.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderDeliveryRepository(OrderDbContext orderDbContext) : IOrderDeliveryRepository
{
    public async Task CreateOrderDeliveryAsync(OrderDelivery orderDelivery, CancellationToken cancellationToken)
    {
        OrderDeliveryEntity entity = orderDelivery.ToEntity();
        await orderDbContext.OrderDeliveries.AddAsync(entity);
    }
}