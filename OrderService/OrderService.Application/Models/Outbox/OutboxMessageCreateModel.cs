using OrderService.Domain.Models.Enums;

namespace OrderService.Application.Models.Outbox;

public record OutboxMessageCreateModel(
Guid Id,
string Topic,
string Key,
string Payload, 
OrderEventType EventType);