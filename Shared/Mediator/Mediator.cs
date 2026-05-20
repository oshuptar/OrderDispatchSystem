using Auth.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Mediator;

public class Mediator(
    IServiceScopeFactory scopeFactory) : IMediator
{
    public async Task ExecuteQueryAsync<T>(T query, CancellationToken cancellationToken)
    {
        await using var scope =  scopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<T>>();
        await handler.HandleQueryAsync(query, cancellationToken);
    }
    
    public async Task<TResult> ExecuteQueryAsync<T, TResult>(T query, CancellationToken cancellationToken)
    {
        await using var scope =  scopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<T, TResult>>();
        return await handler.HandleQueryAsync(query, cancellationToken);
    }

    public async Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken)
    {
        await using var scope =  scopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
        await handler.HandleCommandAsync(command, cancellationToken);
    }

    public async Task<TResult> ExecuteCommandAsync<T, TResult>(T command, CancellationToken cancellationToken)
    {
        await using var scope =  scopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T, TResult>>();
        return await handler.HandleCommandAsync(command, cancellationToken);
    }
}