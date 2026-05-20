using System.Text.Json.Serialization;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class OrderDelivery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    // One-To-One relationship with Order
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public OrderDeliveryStatus OrderDeliveryStatus { get; set; }
    // TODO: move to OrderDeliveryRide. Add Weight and Volume Columns there. This type of entities would be created by OrderDispatcher
    // The status of OrderDeliveryChanges to Completed if all OrderDeliveryRides have been successfully delivered
    public DateTime ScheduledDeliveryDateTime { get; set; }
    public DateTime? DeliveryDateTime { get; set; }
    public Guid? SourceAddressId { get; set; }
    public Address? SourceAddress { get; set; }
    public Guid DestinationAddressId { get; set; }
    public Address? DestinationAddress { get; set; }
}