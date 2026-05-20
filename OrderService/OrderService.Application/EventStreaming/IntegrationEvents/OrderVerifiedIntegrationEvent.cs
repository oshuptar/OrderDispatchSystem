namespace OrderService.Application.EventStreaming.IntegrationEvents;

// When this event is published, the production process starts and notification is sent
// The production service needs a projection of order
public record OrderVerifiedIntegrationEvent(
    Guid OrderId, 
    Guid ClientId);