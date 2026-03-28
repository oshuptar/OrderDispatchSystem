using Auth.Persistence;
using DriverService.Domain.Models.Enums;

namespace DriverService.Infrastructure.Entities;

public sealed class DriverEntity : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 
    
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    
    public DriverAvailability DriverAvailability { get; set; }
    
    public Guid DriverLocationId { get; set; }
    public DriverLocationEntity? DriverLocation { get; set; }
    
    public Guid DriverWorkingScheduleId { get; set; }
    public DriverScheduleEntity? DriverWorkingSchedule { get; set; }
}