using Auth.Persistence;

namespace DriverService.Infrastructure.Entities;

public sealed class DriverLocationEntity : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DriverEntity? Driver { get; set; }
    public string Latitude { get; set; } = String.Empty;
    public string Longitude { get; set; } = String.Empty;
}