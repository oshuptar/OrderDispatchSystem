using OrderService.Application.Features.Address.Update.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;

// TODO: fix - so that OrderId is taken from path parameter
public record ClientOrderUpdateRequest(
    Guid OrderId,
    int? Volume = null,
    int? Weight = null,
    DateTime? RequestedDeliveryDateTime = null,
    ClientAddressUpdateRequest? DestionationAddressUpdateRequest = null
);