using OrderService.Application.Models;

namespace OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;

public record ClientCreateOrderRequestModel(
    int Volume,
    int Weight,
    DateTime ScheduledOrderDateTime,
    AddressRequestModel DeliveryAddress);