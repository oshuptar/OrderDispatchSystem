using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Models;

namespace OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;

public record ClientCreateOrderRequest(
    int Volume,
    int Weight,
    DateTime ScheduledOrderDateTime,
    AddressCreateRequest DeliveryAddressRequest);