using Auth.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Update.Contracts;
using OrderService.Application.Models;
using OrderService.Application.Models.Order;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository(OrderDbContext orderDbContext) : IOrderRepository
{
    public async Task CreateAsync(Order order, CancellationToken cancellationToken)
    {
        OrderEntity entity = order.ToEntity();
        await orderDbContext.AddAsync(entity, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        OrderEntity? order = await GetOrderEntityById(orderId, cancellationToken);
        return order?.ToDomainModel();
    }

    public async Task<bool> ExistsByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await orderDbContext.Orders
            .AnyAsync(order => order.Id == orderId, cancellationToken);
    }

    // Make sure that order exists before retrieving it
    public async Task UpdateAsync(OrderUpdateRequestModel orderUpdateRequestModel, CancellationToken cancellationToken)
    {
        // Worth throwing exception that the order was not found inside repository?
        OrderEntity? order = await GetOrderEntityById(orderUpdateRequestModel.OrderId, cancellationToken);
        order?.OrderStatus = orderUpdateRequestModel.OrderStatus ?? order.OrderStatus;
        order?.ProductionPlantId = orderUpdateRequestModel.ProductionPlantId;
        order?.Volume = orderUpdateRequestModel.Volume ?? order.Volume;
        order?.Weight = orderUpdateRequestModel.Weight ?? order.Weight;
        order?.RequestedDeliveryDateTime = orderUpdateRequestModel.RequestedDeliveryDateTime ?? order.RequestedDeliveryDateTime;
    }

    private async Task<OrderEntity?> GetOrderEntityById(Guid orderId, CancellationToken cancellationToken)
    {
        return await orderDbContext.Orders
            .Where(order => order.Id == orderId)
            .Include(order => order.OrderDelivery)
            .Include(order => order.OrderStatus)
            .FirstOrDefaultAsync(cancellationToken);
    }
}