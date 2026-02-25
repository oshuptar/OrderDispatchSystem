using Auth.Infrastructure;

namespace OrderService.Infrastructure.Entities;

public class OrderDeliveryEntity : Auditable
{
    public Guid Id { get; set; }
    // Many-To-One relationship with Order - one order can be delivered in parts
    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    
    public Guid DriverId { get; set; }
    // No navigational property since Drivers are stored in a different db
    public DateTime ScheduledDeliveryTime { get; set; }
    public DateTime? DeliveryTime { get; set; }
    
    public Guid SourceAddressId { get; set; }
    public AddressEntity? SourceAddress { get; set; }
    
    public Guid DeliveryAddressId { get; set; }
    public AddressEntity? DeliveryAddress { get; set; }
}