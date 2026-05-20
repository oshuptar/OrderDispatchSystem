using System.Text;
using System.Text.Json;
using Auth.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Application.EventStreaming.EventStreaming;
using OrderService.Domain.Models.Enums;

namespace OrderService.Infrastructure.Kafka;

public class KafkaEventProducer : IEventProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventProducer(IOptions<KafkaOptions> options, ILogger<KafkaEventProducer> logger)
    {
        logger.LogInformation($"[{nameof(KafkaEventProducer)}] Bootstrap Server: {options.Value.BootstrapServers}");
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            Acks = options.Value.Acks,
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public async Task ProduceAsync(string topic, string key, string message, OrderEventType eventType, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(message);
        var serializedKey = key;
        await _producer.ProduceAsync(topic, new Message<string, string>()
        {
            Key = serializedKey,
            Value = payload,
            Headers = new Headers
            {
                {"event-type", Encoding.UTF8.GetBytes(eventType.ToString()) }
            }
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}