using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    // No navigation property since Users are stored in a different db
    public Guid UserId { get; set; }
    public Guid? ProductionPlantId { get; set; }
    public ProductionPlant? ProductionPlant { get; set; }
    public OrderStatus OrderStatus { get; set; } 
    public int Volume { get; set; }
    public int Weight { get; set; }
    public required DateTime RequestedDeliveryDateTime { get; set; }
    public OrderDelivery? OrderDelivery { get; set; }
}