using OrderService.Application.Features.Address.Update.Contracts;
using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;

namespace OrderService.Application.Features.OrderDelivery.Update.Contracts;

public record OrderDeliveryUpdateRequest(
    Guid OrderId,
    ClientAddressUpdateRequest? DestionationAddressUpdateRequest = null
    );