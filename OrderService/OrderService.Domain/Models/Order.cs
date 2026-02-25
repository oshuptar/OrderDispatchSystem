namespace OrderService.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    // No navigation property since Users are stored in a different db
    public Guid UserId { get; set; }
    
    public Guid? ProductionPlantId { get; set; }
    public ProductionPlant? ProductionPlant { get; set; }
        
    public int Volume { get; set; }
    public int Weight { get; set; }
    
    public DateTime ScheduledOrderDate { get; set; }
}