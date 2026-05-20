using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Order.Get.Contracts;

public record OrderSearchByRequest(
    Guid? ProductionPlantId, 
    OrderStatus? OrderStatus,
    Guid? UserId,
    DateTime? StartDeliveryDateTime,
    DateTime? EndDeliveryDateTime,
    int Page = 0,
    int Size = 10
    );