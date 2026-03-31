using OrderService.Domain.Enums;

namespace OrderService.Application.Models.Order;

public record OrderSearchRequestModel(
    Guid? ProductionPlantId,
    OrderStatus? OrderStatus,
    Guid? UserId,
    DateTime? StartDeliveryDateTime,
    DateTime? EndDeliveryDateTime
    );