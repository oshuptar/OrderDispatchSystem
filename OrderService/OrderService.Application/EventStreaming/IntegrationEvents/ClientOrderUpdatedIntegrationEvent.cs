namespace OrderService.Application.EventStreaming.IntegrationEvents;

public record ClientOrderUpdatedIntegrationEvent(Guid OrderId, Guid UserId, DateTime UpdatedAt);