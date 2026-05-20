using OrderService.Application.Features.OrderDelivery.Create.Contracts;
using OrderService.Domain.Models;

namespace OrderService.Application.Mappers;

public static class OrderDeliveryMapper
{
    public static OrderDelivery ToDomainModel(this OrderDeliveryCreateRequest model)
    {
        return new OrderDelivery
        {
            ScheduledDeliveryDateTime = model.ScheduledDeliveryDateTime,
            DestinationAddressId = model.DestinationAddressId,
            OrderId = model.OrderId,
        };
    }
}