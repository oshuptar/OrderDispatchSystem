namespace OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;

public record ClientCreateOrderResponse(Guid OrderId, Guid DeliveryId);