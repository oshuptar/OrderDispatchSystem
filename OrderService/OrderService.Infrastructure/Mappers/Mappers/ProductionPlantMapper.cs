using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers.Mappers;

public static class ProductionPlantMapper
{
    public static ProductionPlant ToDomainModel(this ProductionPlantEntity productionPlant)
    {
        return new ProductionPlant
        {
            Id = productionPlant.Id,
            AddressId = productionPlant.AddressId,
            Address = productionPlant.Address?.ToDomainModel(),
        };
    }
}