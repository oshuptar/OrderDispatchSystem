namespace DriverService.Domain.Models;

// Will hold scheduled deliveries for a driver (additionally the entity which stores and tracks driver working hours will be created)
public sealed class DriverSchedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }
}