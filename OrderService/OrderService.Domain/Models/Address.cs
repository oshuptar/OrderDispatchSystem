namespace OrderService.Domain.Models;

public class Address
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Country { get; set; } = String.Empty;
    public string Region { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string? Street { get; set; }
    public string? Apartment { get; set; } = String.Empty;
    public string? PostalCode { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}