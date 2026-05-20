using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
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
        return await orderDbContext.Orders.AnyAsync(order => order.Id == orderId, cancellationToken);
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

    public async Task<int> GetCountAsync(OrderSearchRequestModel searchRequestModel, CancellationToken cancellationToken)
    {
        IQueryable<OrderEntity> query = orderDbContext.Orders;
        if (searchRequestModel.OrderStatus != null) query = query.Where(order => order.OrderStatus == searchRequestModel.OrderStatus);
        if (searchRequestModel.ProductionPlantId != null) query = query.Where(order => order.ProductionPlantId == searchRequestModel.ProductionPlantId);
        if (searchRequestModel.UserId != null) query = query.Where(order => order.UserId == searchRequestModel.UserId);
        if (searchRequestModel.StartDeliveryDateTime != null) query = query.Where(order => order.RequestedDeliveryDateTime >= searchRequestModel.StartDeliveryDateTime);
        if (searchRequestModel.EndDeliveryDateTime != null) query = query.Where(order => order.RequestedDeliveryDateTime <= searchRequestModel.EndDeliveryDateTime);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAsync(OrderSearchRequestModel searchRequestModel, int page, int size, CancellationToken cancellationToken)
    {
        IQueryable<OrderEntity> query = orderDbContext.Orders;
        if (searchRequestModel.OrderStatus != null) query = query.Where(order => order.OrderStatus == searchRequestModel.OrderStatus);
        if (searchRequestModel.ProductionPlantId != null) query = query.Where(order => order.ProductionPlantId == searchRequestModel.ProductionPlantId);
        if (searchRequestModel.UserId != null) query = query.Where(order => order.UserId == searchRequestModel.UserId);
        if (searchRequestModel.StartDeliveryDateTime != null) query = query.Where(order => order.RequestedDeliveryDateTime >= searchRequestModel.StartDeliveryDateTime);
        if (searchRequestModel.EndDeliveryDateTime != null) query = query.Where(order => order.RequestedDeliveryDateTime <= searchRequestModel.EndDeliveryDateTime);
        return await query
            .Skip(page * size)
            .Take(size)
            .Select(order => order.ToDomainModel())
            .ToListAsync(cancellationToken);
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