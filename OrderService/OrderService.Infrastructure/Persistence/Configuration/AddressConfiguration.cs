using Auth.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable(Databases.Order.Tables.Addresses);
        builder.HasKey(address => address.Id);
        builder.Property(address => address.Id).ValueGeneratedNever();
        builder.Property(address => address.Country).IsRequired().HasMaxLength(64);
        builder.Property(address => address.City).IsRequired().HasMaxLength(64);
        builder.Property(address => address.Region).IsRequired().HasMaxLength(64);
        builder.Property(address => address.Street).HasMaxLength(128);
        builder.Property(address => address.Apartment).HasMaxLength(8);
        builder.Property(address => address.PostalCode).HasMaxLength(10);
        // Change to global constant
        builder.Property(address => address.Longitude).HasMaxLength(11);
        builder.Property(address => address.Latitude).HasMaxLength(11);
        
        builder.HasIndex(address => new 
            { 
                address.Country, 
                address.Region, 
                address.City, 
                address.Street, 
                address.Apartment,
                address.PostalCode ,
                address.Longitude,
                address.Latitude
            }).IsUnique();

        builder.HasMany(address => address.SourceOrderDeliveries)
            .WithOne(orderDelivery => orderDelivery.SourceAddress)
            .HasForeignKey(orderDelivery => orderDelivery.SourceAddressId);
        builder.HasMany(address => address.DestinationOrderDeliveries)
            .WithOne(orderDelivery => orderDelivery.DestinationAddress)
            .HasForeignKey(orderDelivery => orderDelivery.DestinationAddressId);
    }
}