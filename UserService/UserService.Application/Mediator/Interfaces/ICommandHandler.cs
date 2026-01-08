namespace UserService.Application.Mediator.Interfaces;

public interface ICommandHandler<in T>
{
    Task HandleCommand(T command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in T, TResult>
{
    Task<TResult> HandleCommand(T command, CancellationToken cancellationToken);
}