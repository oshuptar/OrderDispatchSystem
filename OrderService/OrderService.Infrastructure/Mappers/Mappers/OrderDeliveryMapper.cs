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
            DestinationAddress = entity.DestinationAddress?.ToDomainModel(),
            DestinationAddressId = entity.DestinationAddressId,
            DeliveryDateTime = entity.DeliveryDateTime,
            ScheduledDeliveryDateTime = entity.ScheduledDeliveryDateTime,
            OrderId = entity.OrderId,
            Order = entity.Order?.ToDomainModel(),
            SourceAddressId = entity.SourceAddressId,
            SourceAddress = entity.SourceAddress?.ToDomainModel(),
            OrderDeliveryStatus = entity.OrderDeliveryStatus,
        };
    }
    
    public static OrderDeliveryEntity ToEntity(this OrderDelivery model)
    {
        return new OrderDeliveryEntity
        {
            Id = model.Id,
            DestinationAddressId = model.DestinationAddressId,
            DeliveryDateTime = model.DeliveryDateTime,
            ScheduledDeliveryDateTime = model.ScheduledDeliveryDateTime,
            OrderId = model.OrderId,
            SourceAddressId = model.SourceAddressId,
            OrderDeliveryStatus = model.OrderDeliveryStatus,
        };
    }
}