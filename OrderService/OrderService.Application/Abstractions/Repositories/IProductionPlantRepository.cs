namespace OrderService.Application.Abstractions.Repositories;

public interface IProductionPlantRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
}