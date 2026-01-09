namespace UserService.Application.Mediator.Interfaces;

public interface IQueryHandler<in T>
{
    Task HandleQueryAsync(T query, CancellationToken cancellationToken);
}

public interface IQueryHandler<in T, TResult>
{
    Task<TResult> HandleQueryAsync(T command, CancellationToken cancellationToken);
}