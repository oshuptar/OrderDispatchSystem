using Auth.Infrastructure;

namespace OrderService.Infrastructure.Entities;

public class OrderEntity : Auditable
{
        public Guid Id { get; set; }
        // No navigation property since Users are stored in a different db
        public Guid UserId { get; set; }
    
        public Guid? ProductionPlantId { get; set; }
        public ProductionPlantEntity? ProductionPlant { get; set; }
        
        public int Volume { get; set; }
        public int Weight { get; set; }
    
        public DateTime ScheduledOrderDate { get; set; }
}