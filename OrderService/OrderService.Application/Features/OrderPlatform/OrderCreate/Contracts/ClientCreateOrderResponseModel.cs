namespace OrderService.Application.Features.OrderPlatform.OrderCreate.Contracts;

public record ClientCreateOrderResponseModel(Guid OrderId, Guid DeliveryId);