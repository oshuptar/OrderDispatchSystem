using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.EventStreaming.EventStreaming;

namespace OrderService.Application.Workers;

public class OutboxPublisherWorker (
    IServiceProvider serviceProvider, 
    ILogger<OutboxPublisherWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"{nameof(OutboxPublisherWorker)} is starting...");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
                await using var scope = serviceProvider.CreateAsyncScope();
                var outboxMessageRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
                var producer = scope.ServiceProvider.GetRequiredService<IEventProducer>();

                var events = await outboxMessageRepository.GetOutboxMessagesAsync(100, stoppingToken);
                foreach (var @event in events)
                {
                    await producer.ProduceAsync(
                        @event.Topic,
                        @event.Key,
                        @event.Payload,
                        @event.EventType,
                        stoppingToken);
                    logger.LogInformation($"[{DateTime.UtcNow}]: Event published: {@event.EventType}");
                }

                await outboxMessageRepository.MarkOutboxMessagesAsProcessedAsync(events.Select(e => e.Id),
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[{DateTime.UtcNow}]: Error occurred while publishing events");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        logger.LogInformation($"{nameof(OutboxPublisherWorker)} is stopping...");
    }
}