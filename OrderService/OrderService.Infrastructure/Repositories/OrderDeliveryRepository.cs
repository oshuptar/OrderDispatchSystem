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

    public async Task UpdateAsync(OrderDeliveryUpdateRequestModel request, CancellationToken cancellationToken)
    {
        var orderDelivery = await orderDbContext.OrderDeliveries
            .Where(productionPlant => productionPlant.Id == request.OrderDeliveryId)
            .FirstOrDefaultAsync(cancellationToken);
        // We assume that in command handler we have already checked that order delivery exists
        if (orderDelivery is not null)
        {
            orderDelivery.SourceAddressId = request.SourceAddressId ?? orderDelivery.SourceAddressId;
            orderDelivery.DestinationAddressId = request.DestinationAddressId ?? orderDelivery.DestinationAddressId;
            orderDelivery.OrderDeliveryStatus = request.OrderDeliveryStatus ?? orderDelivery.OrderDeliveryStatus;
            orderDelivery.ScheduledDeliveryDateTime =
                request.RequestedDeliveryDateTime ?? orderDelivery.ScheduledDeliveryDateTime;
            await orderDbContext.SaveChangesAsync(cancellationToken);
        }
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