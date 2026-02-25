using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers;

public static class OrderDeliveryMapper
{
    public static OrderDelivery ToDomainModel(this OrderDeliveryEntity entity)
    {
        return new OrderDelivery
        {
            Id = entity.Id,
            DeliveryAddress = entity.DeliveryAddress?.ToDomainModel(),
            DeliveryAddressId = entity.DeliveryAddressId,
            DeliveryTime = entity.DeliveryTime,
            ScheduledDeliveryTime = entity.ScheduledDeliveryTime,
            OrderId = entity.OrderId,
            Order = entity.Order?.ToDomainModel(),
            DriverId = entity.DriverId,
            SourceAddressId = entity.SourceAddressId,
            SourceAddress = entity.SourceAddress?.ToDomainModel()
        };
    }
}