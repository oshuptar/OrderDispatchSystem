using Auth.Mediator.Interfaces;
using OrderService.Application.Features.ProductionPlant.Get.Contracts;

namespace OrderService.Application.Features.ProductionPlant.Get;

public class ProductionPlantGetByIdQueryHandler : IQueryHandler<ProductionPlantGetByIdRequest>
{
    public Task HandleQueryAsync(ProductionPlantGetByIdRequest query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}