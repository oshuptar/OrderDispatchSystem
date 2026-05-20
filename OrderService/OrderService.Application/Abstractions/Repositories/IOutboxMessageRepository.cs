using OrderService.Application.Models.Outbox;

namespace OrderService.Application.Abstractions.Repositories;

public interface IOutboxMessageRepository
{ 
    Task CreateAsync(OutboxMessageCreateModel messageCreate, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OutboxMessageModel>> GetOutboxMessagesAsync(int batchSize, CancellationToken cancellationToken);
    Task MarkOutboxMessagesAsProcessedAsync(IEnumerable<Guid> messageIds, CancellationToken cancellationToken);
}