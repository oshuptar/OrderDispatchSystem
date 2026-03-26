using System.Text.Json;
using Auth.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.Application.Abstractions.EventStreaming;

namespace OrderService.Infrastructure.Kafka;

// TODO: implement KafkaEventProducer
public class KafkaEventProducer : IEventProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventProducer(IOptions<KafkaOptions> options)
    {
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public async Task ProduceAsync<K, V>(string topic, K key, V message, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(message);
        var serializedKey = key?.ToString() ?? string.Empty;
        await _producer.ProduceAsync(topic, new Message<string, string>()
        {
            Key = serializedKey,
            Value = payload
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}