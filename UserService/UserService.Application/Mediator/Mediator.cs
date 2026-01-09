using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Mediator.Interfaces;

namespace UserService.Application.Mediator;

public class Mediator(
    IServiceScopeFactory scopeFactory) : IMediator
{
    public Task ExecuteQueryAsync<T>(T query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public Task<TResult> ExecuteQueryAsync<T, TResult>(T query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteCommandAsync<T>(T command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TResult> ExecuteCommandAsync<T, TResult>(T command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}