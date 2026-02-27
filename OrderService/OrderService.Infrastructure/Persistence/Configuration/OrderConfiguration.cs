using Auth.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable(Databases.Order.Tables.Orders);
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id).ValueGeneratedOnAdd();
        builder.Property(order => order.UserId).IsRequired();
        builder.Property(order => order.Volume).IsRequired();
        builder.Property(order => order.Weight).IsRequired();
        builder.Property(order => order.ScheduledOrderDateTime).IsRequired();

        builder.HasIndex(order => order.UserId);
    }
}
