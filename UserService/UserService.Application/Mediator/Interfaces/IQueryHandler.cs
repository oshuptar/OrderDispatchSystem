namespace UserService.Application.Mediator.Interfaces;

public interface IQueryHandler<in T>
{
    Task HandleQuery(T query, CancellationToken cancellationToken);
}

public interface IQueryHandler<in T, TResult>
{
    Task<TResult> HandleQuery(T command, CancellationToken cancellationToken);
}