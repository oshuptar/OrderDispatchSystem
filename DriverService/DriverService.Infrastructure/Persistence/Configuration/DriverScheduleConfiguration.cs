using DriverService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverService.Infrastructure.Persistence.Configuration;

public class DriverScheduleConfiguration : IEntityTypeConfiguration<DriverScheduleEntity>
{
    public void Configure(EntityTypeBuilder<DriverScheduleEntity> builder)
    {
        throw new NotImplementedException();
    }
}