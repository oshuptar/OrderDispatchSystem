using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Models.Outbox;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OutboxMessageRepository (
    OrderDbContext orderDbContext
    ) : IOutboxMessageRepository
{
    public async Task CreateAsync(OutboxMessageCreateModel messageCreate, CancellationToken cancellationToken)
    {
        OutboxMessageEntity entity = messageCreate.ToEntity();
        await orderDbContext.OutboxMessages.AddAsync(entity, cancellationToken);
    }
    public async Task<IReadOnlyCollection<OutboxMessageModel>> GetOutboxMessagesAsync(int batchSize, CancellationToken cancellationToken)
    {
        return await orderDbContext.OutboxMessages
            .Where(message => message.ProcessedAt == null)
            .OrderBy(message => message.CreatedAt)
            .Take(batchSize)
            .AsNoTracking()
            .Select(message => message.ToModel())
            .ToListAsync(cancellationToken);
    }

    public async Task MarkOutboxMessagesAsProcessedAsync(IEnumerable<Guid> messageIds, CancellationToken cancellationToken)
    {
        // Executes directly without loading entities into memory
        await orderDbContext.OutboxMessages
            .Where(message => messageIds.Contains(message.Id))
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    message => message.ProcessedAt,
                    DateTime.UtcNow), cancellationToken);
        
        // var processedEntities = await orderDbContext.OutboxMessages
        //     .Where(message => messageIds.Contains(message.Id))
        //     .ToListAsync(cancellationToken);
        // processedEntities.ForEach(message => message.ProcessedAt = DateTime.UtcNow);
        // await orderDbContext.SaveChangesAsync(cancellationToken);
    }
}