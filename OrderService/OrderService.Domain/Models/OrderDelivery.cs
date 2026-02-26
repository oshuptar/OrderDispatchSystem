using System.Text.Json.Serialization;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class OrderDelivery
{
    public Guid Id { get; set; }
    // Many-To-One relationship with Order - one order can be delivered in parts
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    
    public OrderDeliveryStatus OrderDeliveryStatus { get; set; }
    
    public Guid DriverId { get; set; }
    // No navigational property since Drivers are stored in a different db
    public DateTime ScheduledDeliveryTime { get; set; }
    public DateTime? DeliveryTime { get; set; }
    
    public Guid SourceAddressId { get; set; }
    public Address? SourceAddress { get; set; }
    
    public Guid DeliveryAddressId { get; set; }
    public Address? DeliveryAddress { get; set; }
}