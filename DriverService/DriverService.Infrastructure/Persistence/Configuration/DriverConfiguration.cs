using DriverService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverService.Infrastructure.Persistence.Configuration;

public class DriverConfiguration : IEntityTypeConfiguration<DriverEntity>
{
    public void Configure(EntityTypeBuilder<DriverEntity> builder)
    {
        throw new NotImplementedException();
    }
}