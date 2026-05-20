namespace OrderService.Domain.Enums;

public enum OrderDeliveryStatus
{
    Created,
    Scheduled,
    InProgress,
    Delivered,
    Cancelled
}