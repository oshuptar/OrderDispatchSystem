namespace OrderService.Domain.Models;

public class Address
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