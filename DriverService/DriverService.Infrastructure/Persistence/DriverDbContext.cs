using Microsoft.EntityFrameworkCore;

namespace DriverService.Infrastructure.Persistence;

public sealed class DriverDbContext(DbContextOptions<DriverDbContext> dbContextOptions) 
    : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DriverDbContext).Assembly);
    }
}