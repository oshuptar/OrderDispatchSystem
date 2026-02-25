using Auth.Infrastructure;

namespace OrderService.Infrastructure.Entities;

public class AddressEntity : Auditable
{
    public Guid Id {get; set;}
    public required String Country { get; set; }
    public String? Region { get; set; }
    public required String City { get; set; }
    public String? Street { get; set; }
    public String? PostalCode { get; set; }
    public String? Longitude { get; set; }
    public String? Latitude { get; set; }
}