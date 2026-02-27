using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers.Mappers;

public static class OrderMapper
{
    public static Order ToDomainModel(this OrderEntity entity)
    {
        return new Order()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProductionPlant = entity.ProductionPlant?.ToDomainModel(),
            ProductionPlantId = entity.ProductionPlantId,
            Volume = entity.Volume,
            Weight = entity.Weight,
            ScheduledOrderDateTime = entity.ScheduledOrderDateTime,
            OrderStatus = entity.OrderStatus,
            OrderDelivery = entity.OrderDelivery?.ToDomainModel()
        };
    }
}