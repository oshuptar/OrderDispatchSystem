using Auth.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Update.Contracts;
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

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        OrderEntity? order = await orderDbContext.Orders
            .Where(order => order.Id == orderId)
            .Include(order => order.OrderDelivery)
            .Include(order => order.OrderStatus)
            .FirstOrDefaultAsync(cancellationToken);
        return order?.ToDomainModel();
    }
}