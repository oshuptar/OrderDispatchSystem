using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Order.Update.Contracts;

public record OrderUpdateRequest(
    Guid OrderId,
    OrderStatus? OrderStatus = null,
    int? Volume = null,
    int? Weight = null,
    DateTime? RequestedDeliveryDateTime = null,
    Guid? ProductionPlantId = null
    );