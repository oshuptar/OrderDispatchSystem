using OrderService.Domain.Enums;

namespace OrderService.Application.Models.Order;

public record OrderUpdateRequestModel(
    Guid OrderId,
    OrderStatus? OrderStatus = null,
    int? Volume = null,
    int? Weight = null,
    DateTime? RequestedDeliveryDateTime = null,
    Guid? ProductionPlantId = null
);