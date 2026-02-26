using Auth.Infrastructure;

namespace OrderService.Infrastructure.Entities;

public class ProductionPlantEntity : Auditable
{
    public Guid Id { get; set; }
    public required Guid AddressId { get; set; }
    public AddressEntity? Address { get; set; }
}