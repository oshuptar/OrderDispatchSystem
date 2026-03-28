using Auth.Persistence;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Context;
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
    public DbSet<OutboxMessageEntity> OutboxMessages => Set<OutboxMessageEntity>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }
}