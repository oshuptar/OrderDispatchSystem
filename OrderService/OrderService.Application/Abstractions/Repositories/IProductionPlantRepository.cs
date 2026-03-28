using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IProductionPlantRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProductionPlant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}