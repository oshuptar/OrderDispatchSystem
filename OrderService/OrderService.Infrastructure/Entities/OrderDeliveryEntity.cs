using Auth.Persistence;
using OrderService.Domain.Enums;

namespace OrderService.Infrastructure.Entities;

public class OrderDeliveryEntity : Auditable
{
    public Guid Id { get; set; }
    // One-to-One relationship with Order
    public required Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    public required OrderDeliveryStatus OrderDeliveryStatus { get; set; } = OrderDeliveryStatus.Created;
    // The predicted delivery time. TODO: determine if needed
    public required DateTime ScheduledDeliveryDateTime { get; set; }
    // Stores the actual delivery time for history
    public DateTime? DeliveryDateTime { get; set; }
    public required Guid? SourceAddressId { get; set; }
    public AddressEntity? SourceAddress { get; set; }
    public required Guid DestinationAddressId { get; set; }
    public AddressEntity? DestinationAddress { get; set; }
}