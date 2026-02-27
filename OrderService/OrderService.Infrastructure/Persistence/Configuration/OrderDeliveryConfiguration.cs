using Auth.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDeliveryEntity>
{
    public void Configure(EntityTypeBuilder<OrderDeliveryEntity> builder)
    {
        builder.ToTable(Databases.Order.Tables.OrderDeliveries).HasKey(delivery => delivery.Id);
        builder.Property(delivery => delivery.Id).ValueGeneratedOnAdd();
        builder.Property(delivery => delivery.OrderId).IsRequired();
        builder.Property(delivery => delivery.OrderDeliveryStatus).IsRequired();
        builder.Property(delivery => delivery.DriverId).IsRequired();
        builder.Property(delivery => delivery.ScheduledDeliveryDateTime).IsRequired();
        builder.Property(delivery => delivery.SourceAddressId).IsRequired();
        builder.Property(delivery => delivery.DeliveryAddressId).IsRequired();

        builder.HasIndex(delivery => delivery.OrderId);
    }
}