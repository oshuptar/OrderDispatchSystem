using Auth.Persistence;
using OrderService.Domain.Enums;

namespace OrderService.Infrastructure.Entities;

public class OrderEntity : Auditable
{
        public Guid Id { get; set; }
        // No navigation property since Users are stored in a different db
        public required Guid UserId { get; set; }
        public required OrderStatus OrderStatus { get; set; } = OrderStatus.Created;
        // ProductionPlant may not be immediately set
        public Guid? ProductionPlantId { get; set; }
        public ProductionPlantEntity? ProductionPlant { get; set; }
        public required int Volume { get; set; }
        public required int Weight { get; set; }
        public OrderDeliveryEntity? OrderDelivery { get; set; }
        public required DateTime RequestedDeliveryDateTime { get; set; }
}