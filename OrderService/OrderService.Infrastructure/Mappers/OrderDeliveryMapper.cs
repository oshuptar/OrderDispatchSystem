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
            DeliveryTime = entity.DeliveryDateTime,
            ScheduledDeliveryTime = entity.ScheduledDeliveryDateTime,
            OrderId = entity.OrderId,
            Order = entity.Order?.ToDomainModel(),
            DriverId = entity.DriverId,
            SourceAddressId = entity.SourceAddressId,
            SourceAddress = entity.SourceAddress?.ToDomainModel(),
            OrderDeliveryStatus = entity.OrderDeliveryStatus,
        };
    }
}