namespace OrderService.Application.Features.OrderDelivery.Create.Contracts;

public record OrderDeliveryCreateRequest(
    Guid OrderId,
    DateTime ScheduledDeliveryDateTime,
    Guid DestinationAddressId
    );