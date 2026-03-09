using Auth.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderService.Application.Context;

namespace OrderService.Infrastructure.Interceptors;

public class AuditInterceptor(UserContext userContext) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Audit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
       Audit(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private void Audit(DbContext? dbContext)
    {
        if (dbContext is null) return;
        
        var entries = dbContext.ChangeTracker.Entries<Auditable>();
        var now = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.IsDeleted = false;
                // should exist, whenever HttpContext exists
                entry.Entity.CreatedBy = userContext.User!.Id;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                // should exist, whenever HttpContext exists
                entry.Entity.LastUpdatedBy = userContext.User!.Id;
            }
        }
    }
    
}