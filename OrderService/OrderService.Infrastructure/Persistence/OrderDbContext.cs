using Auth.Persistence;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence;

public class OrderDbContext(
    DbContextOptions<OrderDbContext> dbContextOptions
    ) : DbContext(dbContextOptions)
{
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<OrderDeliveryEntity> OrderDeliveries => Set<OrderDeliveryEntity>();
    public DbSet<ProductionPlantEntity> ProductionPlants => Set<ProductionPlantEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        Audit();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void Audit()
    {
        var entries = ChangeTracker
            .Entries<Auditable>();

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.IsDeleted = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}