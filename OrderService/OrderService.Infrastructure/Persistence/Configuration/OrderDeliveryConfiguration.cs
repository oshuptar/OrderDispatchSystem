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
        builder.Property(delivery => delivery.Id).ValueGeneratedNever();
        builder.Property(delivery => delivery.OrderId).IsRequired();
        builder.Property(delivery => delivery.OrderDeliveryStatus).IsRequired();
        builder.Property(delivery => delivery.ScheduledDeliveryDateTime).IsRequired();
        builder.Property(delivery => delivery.DestinationAddressId).IsRequired();

        builder.HasIndex(delivery => delivery.OrderId);
        builder.HasQueryFilter(delivery => !delivery.IsDeleted);
        
        // One-to-One relationShip with Order
        builder.HasOne(orderDelivery => orderDelivery.Order)
            .WithOne(order => order.OrderDelivery)
            .HasForeignKey<OrderDeliveryEntity>(orderDelivery => orderDelivery.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}