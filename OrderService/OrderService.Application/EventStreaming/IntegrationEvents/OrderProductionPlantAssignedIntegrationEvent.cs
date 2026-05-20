namespace OrderService.Application.EventStreaming.IntegrationEvents;

public record OrderProductionPlantAssignedIntegrationEvent(Guid OrderId, Guid ProductionPlantId, DateTime AssignedAt);