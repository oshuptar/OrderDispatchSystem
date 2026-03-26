namespace OrderService.Application.Abstractions.EventStreaming;

public interface IEventProducer
{
    Task ProduceAsync<K, V>(string topic, K key, V message, CancellationToken cancellationToken);
}