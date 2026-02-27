using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers.Mappers;

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
            ScheduledDeliveryDateTime = entity.ScheduledDeliveryDateTime,
            OrderId = entity.OrderId,
            Order = entity.Order?.ToDomainModel(),
            SourceAddressId = entity.SourceAddressId,
            SourceAddress = entity.SourceAddress?.ToDomainModel(),
            OrderDeliveryStatus = entity.OrderDeliveryStatus,
        };
    }
}