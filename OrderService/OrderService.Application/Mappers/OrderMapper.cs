using OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;
using OrderService.Domain.Models;

namespace OrderService.Application.Mappers;

public static class OrderMapper
{
    public static Order ToDomainModel(this ClientCreateOrderRequestModel model)
    {
        return new Order()
        {
            Volume = model.Volume,
            Weight = model.Weight,
            ScheduledOrderDateTime = model.ScheduledOrderDateTime
        };
    }
}