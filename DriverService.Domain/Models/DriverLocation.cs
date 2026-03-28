namespace DriverService.Domain.Models;

public sealed class DriverLocation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Driver? Driver { get; set; }
    public string Latitude { get; set; } = String.Empty;
    public string Longitude { get; set; } = String.Empty;
}