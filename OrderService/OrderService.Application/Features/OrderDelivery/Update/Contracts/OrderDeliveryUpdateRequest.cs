using OrderService.Application.Features.Address.Update.Contracts;

namespace OrderService.Application.Features.OrderDelivery.Update.Contracts;

public record OrderDeliveryUpdateRequest(
    Guid OrderId,
    AddressUpdateRequest? DestionationAddressUpdateRequest = null
    );