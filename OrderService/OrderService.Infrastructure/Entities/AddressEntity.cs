using Auth.Persistence;

namespace OrderService.Infrastructure.Entities;

public class AddressEntity : Auditable
{
    public Guid Id {get; set;}
    public required string Country { get; set; }
    public required string Region { get; set; }
    public required string City { get; set; }
    public string? Street { get; set; }
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public List<OrderDeliveryEntity>? SourceOrderDeliveries { get; set; }
    public List<OrderDeliveryEntity>? DestinationOrderDeliveries { get; set; }
}