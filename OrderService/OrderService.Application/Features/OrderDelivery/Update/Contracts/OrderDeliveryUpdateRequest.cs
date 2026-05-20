using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.OrderDelivery.Update.Contracts;

public record OrderDeliveryUpdateRequest(
    Guid OrderId,
    DateTime? RequestedDeliveryDateTime = null,
    ClientAddressUpdateRequest? DestionationAddressUpdateRequest = null,
    OrderDeliveryStatus? OrderDeliveryStatus = null,
    Guid? SourceAddressId = null
    );