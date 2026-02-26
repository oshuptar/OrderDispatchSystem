using OrderService.Application.Abstractions.Repositories;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class ProductionPlantRepository(OrderDbContext orderDbContext) : IProductionPlantRepository
{
    
}