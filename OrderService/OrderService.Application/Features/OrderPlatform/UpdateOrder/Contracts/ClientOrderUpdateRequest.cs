using OrderService.Application.Features.Address.Update.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;

public record ClientOrderUpdateRequest(
    Guid OrderId,
    // Update DeliveryAddressDto
    OrderStatus? OrderStatus = null,
    int? Volume = null,
    int? Weight = null,
    DateTime? RequestedDeliveryDateTime = null,
    AddressUpdateRequest? DestionationAddressUpdateRequest = null,
    Guid? ProductionPlantId = null
);