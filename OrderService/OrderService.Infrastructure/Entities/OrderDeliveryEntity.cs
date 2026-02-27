using Auth.Infrastructure;
using OrderService.Domain.Enums;

namespace OrderService.Infrastructure.Entities;

public class OrderDeliveryEntity : Auditable
{
    public Guid Id { get; set; }
    // Many-To-One relationship with Order - one order can be delivered in parts
    public required Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }

    public required OrderDeliveryStatus OrderDeliveryStatus { get; set; } = OrderDeliveryStatus.Scheduled;
    
    public Guid DriverId { get; set; }
    // No navigational property since Drivers are stored in a different db
    
    // The predicted delivery time. TODO: determine if needed
    public required DateTime ScheduledDeliveryDateTime { get; set; }
    
    // Stores the actual delivery time for history
    public DateTime? DeliveryDateTime { get; set; }
    
    public required Guid SourceAddressId { get; set; }
    public AddressEntity? SourceAddress { get; set; }
    
    public required Guid DeliveryAddressId { get; set; }
    public AddressEntity? DeliveryAddress { get; set; }
}