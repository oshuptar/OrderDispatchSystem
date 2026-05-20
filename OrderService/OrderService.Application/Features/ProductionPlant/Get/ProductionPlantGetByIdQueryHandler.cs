using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.ProductionPlant.Get.Contracts;

namespace OrderService.Application.Features.ProductionPlant.Get;

public class ProductionPlantGetByIdQueryHandler (
    IProductionPlantRepository productionPlantRepository
    ) : IQueryHandler<ProductionPlantGetByIdRequest, Domain.Models.ProductionPlant>
{
    public async Task<Domain.Models.ProductionPlant> HandleQueryAsync(ProductionPlantGetByIdRequest command, CancellationToken cancellationToken)
    {
        Domain.Models.ProductionPlant? productionPlant =
            await productionPlantRepository.GetByIdAsync(command.Guid, cancellationToken);
        if (productionPlant is null)
            throw new NotFoundException($"Production plant with id: {command.Guid} not found");
        return productionPlant;
    }
}