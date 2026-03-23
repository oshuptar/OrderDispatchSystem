using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Models.OrderDelivery;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderDeliveryRepository(OrderDbContext orderDbContext) : IOrderDeliveryRepository
{
    public async Task CreateAsync(OrderDelivery orderDelivery, CancellationToken cancellationToken)
    {
        OrderDeliveryEntity entity = orderDelivery.ToEntity();
        await orderDbContext.OrderDeliveries.AddAsync(entity);
    }

    public Task UpdateAsync(OrderDeliveryUpdateRequestModel request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDelivery?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        // This is possible since the relationship is one-to-one
        OrderDeliveryEntity? entity = await orderDbContext.OrderDeliveries
            .Where(orderDelivery => orderDelivery.OrderId == orderId)
            .Include(orderDelivery => orderDelivery.Order)
            .Include(orderDelivery => orderDelivery.DestinationAddress)
            .FirstOrDefaultAsync(cancellationToken);
        return entity?.ToDomainModel();
    }
}