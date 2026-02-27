namespace OrderService.Domain.Models;

public class ProductionPlant
{
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
}