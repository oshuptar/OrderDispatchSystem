using Microsoft.EntityFrameworkCore.Storage;

namespace UserService.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
}