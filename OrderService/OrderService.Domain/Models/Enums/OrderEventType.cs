namespace OrderService.Domain.Models.Enums;

public enum OrderEventType
{
    ClientOrderCreated = 0,
    ClientOrderUpdated,
    OrderVerified,
    OrderDeleted,
}