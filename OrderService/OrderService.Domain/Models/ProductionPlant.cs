namespace OrderService.Domain.Models;

public class ProductionPlant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
}