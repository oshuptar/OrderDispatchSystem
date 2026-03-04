using Auth.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class ProductionPlantConfiguration : IEntityTypeConfiguration<ProductionPlantEntity>
{
    public void Configure(EntityTypeBuilder<ProductionPlantEntity> builder)
    {
        builder.ToTable(Databases.Order.Tables.ProductionPlants).HasKey(productionPlant => productionPlant.Id);
        builder.Property(productionPlant => productionPlant.Id).ValueGeneratedNever();
        builder.Property(productionPlant => productionPlant.AddressId).IsRequired();

        builder.HasMany(productionPlant => productionPlant.Orders)
            .WithOne(order => order.ProductionPlant)
            .HasForeignKey(order => order.ProductionPlantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}