using OrderService.Application.Features.Address.Create.Contracts;

namespace OrderService.Application.Features.OrderPlatform.PostOrder.Contracts;

public record ClientCreateOrderRequest(
    int Volume,
    int Weight,
    DateTime ScheduledOrderDateTime,
    AddressCreateRequest DeliveryAddressRequest);