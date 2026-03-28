using OrderService.Domain.Models.Enums;

namespace OrderService.Application.EventStreaming.EventStreaming;

public interface IEventProducer
{
    Task ProduceAsync(string topic, string key, string message, OrderEventType eventType ,CancellationToken cancellationToken);
}