using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using UserService.Application.UnitOfWork.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public class UnitOfWork(UserDbContext userDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await userDbContext.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await userDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        await transaction.CommitAsync(cancellationToken);
    }
    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        await transaction.RollbackAsync(cancellationToken);
    }
}