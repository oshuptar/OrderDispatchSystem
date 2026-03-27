using OrderService.Domain.Models.Enums;

namespace OrderService.Application.Models.Outbox;

public record OutboxMessageModel(
    Guid Id,
    string Topic,
    string Key,
    string Payload,
    string? Error,
    OrderEventType EventType,
    DateTime? ProcessedAt,
    DateTime? CreatedAt
    );