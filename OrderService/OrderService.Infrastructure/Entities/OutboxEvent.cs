using Auth.Persistence;
using OrderService.Domain.Models.Enums;

namespace OrderService.Infrastructure.Entities;

public class OutboxEvent : Auditable
{
    public Guid Id { get; set; }
    public required string Topic { get; set; }
    public required string Key { get; set; }
    public required string Payload { get; set; }
    public required OrderEventType EventType { get; set; }
    public DateTime? ProcessedAt { get; set; }
}