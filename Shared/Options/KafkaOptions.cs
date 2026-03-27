using Confluent.Kafka;

namespace Auth.Options;

// TODO: define KafkaOptions
public sealed class KafkaOptions
{
    public const String SectionName = "Kafka";
    public required string BootstrapServers { get; init; }
    public Acks Acks { get; init; } = Acks.All;
}