using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class ProductionPlantRepository(OrderDbContext orderDbContext) : IProductionPlantRepository
{
    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await orderDbContext.ProductionPlants.AnyAsync(productionPlant => productionPlant.Id == id, cancellationToken);
    }

    public async Task<ProductionPlant?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ProductionPlantEntity? entity = await orderDbContext.ProductionPlants
            .Where(productionPlant => productionPlant.Id == id)
            .Include(productionPlant => productionPlant.Address)
            .FirstOrDefaultAsync(cancellationToken);
        return entity?.ToDomainModel();
    }
}