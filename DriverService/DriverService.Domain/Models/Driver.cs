using DriverService.Domain.Models.Enums;

namespace DriverService.Domain.Models;

public sealed class Driver
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } // for consistency
    
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    
    public DriverAvailability DriverAvailability { get; set; }
    
    public Guid DriverLocationId { get; set; }
    public DriverLocation? DriverLocation { get; set; }
    
    public Guid DriverWorkingScheduleId { get; set; }
    public DriverSchedule? DriverWorkingSchedule { get; set; }
}