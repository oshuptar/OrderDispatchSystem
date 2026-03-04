using OrderService.Application.Features.Address.Create.Contracts;

namespace OrderService.Application.Features.Order.Create.Contracts;

public record OrderCreateRequest(
    Guid UserId,
    int Volume,
    int Weight,
    DateTime RequestedDeliveryDateTime,
    Guid DestinationAddressId
    );