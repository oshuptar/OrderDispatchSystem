using Auth.Persistence;

namespace DriverService.Infrastructure.Entities;

public sealed class DriverScheduleEntity : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DriverId { get; set; }
    public DriverEntity? Driver { get; set; }
}