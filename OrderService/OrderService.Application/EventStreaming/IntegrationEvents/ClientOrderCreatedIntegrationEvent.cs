namespace OrderService.Application.EventStreaming.IntegrationEvents;

public record ClientOrderCreatedIntegrationEvent(Guid OrderId, Guid UserId, DateTime CreatedAt);