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
            OrderStatus = entity.OrderStatus,
            OrderDelivery = entity.OrderDelivery?.ToDomainModel(),
            RequestedDeliveryDateTime = entity.RequestedDeliveryDateTime,
        };
    }

    public static OrderEntity ToEntity(this Order model)
    {
        return new OrderEntity()
        {
            Id = model.Id,
            UserId = model.UserId,
            ProductionPlantId = model.ProductionPlantId,
            Volume = model.Volume,
            Weight = model.Weight,
            OrderStatus = model.OrderStatus,
            RequestedDeliveryDateTime = model.RequestedDeliveryDateTime,
        };
    }
}