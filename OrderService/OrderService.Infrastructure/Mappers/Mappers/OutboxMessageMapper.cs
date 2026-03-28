using OrderService.Application.Models.Outbox;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers.Mappers;

public static class OutboxMessageMapper
{
    public static OutboxMessageEntity ToEntity(this OutboxMessageCreateModel createModel)
    {
        return new OutboxMessageEntity
        {
            Id = createModel.Id,
            Topic = createModel.Topic,
            Key = createModel.Key,
            Payload = createModel.Payload,
            EventType = createModel.EventType,
        };
    }

    public static OutboxMessageModel ToModel(this OutboxMessageEntity entity)
    {
        return new OutboxMessageModel(entity.Id,
            entity.Topic,
            entity.Key,
            entity.Payload,
            entity.Error,
            entity.EventType,
            entity.ProcessedAt,
            entity.CreatedAt
        );
    }
}