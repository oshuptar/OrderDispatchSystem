using UserService.Application.UnitOfWork.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public class UnitOfWork(UserDbContext userContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await userContext.SaveChangesAsync(cancellationToken);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        await action(cancellationToken);
        await userContext.SaveChangesAsync(cancellationToken);
    }
}