namespace Auth.Mediator.Interfaces;

public interface ICommandHandler<in T>
{
    Task HandleCommandAsync(T command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in T, TResult>
{
    Task<TResult> HandleCommandAsync(T command, CancellationToken cancellationToken);
}