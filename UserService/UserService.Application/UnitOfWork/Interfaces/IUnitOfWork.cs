using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace UserService.Application.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
}