using DriverService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverService.Infrastructure.Persistence.Configuration;

public class DriverLocationConfiguration : IEntityTypeConfiguration<DriverLocationEntity>
{
    public void Configure(EntityTypeBuilder<DriverLocationEntity> builder)
    {
        throw new NotImplementedException();
    }
}