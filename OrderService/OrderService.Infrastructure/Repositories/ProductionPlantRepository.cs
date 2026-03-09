using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class ProductionPlantRepository(OrderDbContext orderDbContext) : IProductionPlantRepository
{
    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await orderDbContext.ProductionPlants.AnyAsync(productionPlant => productionPlant.Id == id, cancellationToken);
    }
}