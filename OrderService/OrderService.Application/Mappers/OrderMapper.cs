using OrderService.Application.Features.Order.Create.Contracts;
using OrderService.Domain.Models;

namespace OrderService.Application.Mappers;

public static class OrderMapper
{
    public static Order ToDomainModel(this OrderCreateRequest model)
    {
        return new Order()
        {
            UserId =  model.UserId,
            Volume = model.Volume,
            Weight = model.Weight,
            RequestedDeliveryDateTime = model.RequestedDeliveryDateTime
        };
    }
}