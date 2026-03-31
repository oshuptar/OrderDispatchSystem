using OrderService.Application.Models;
using OrderService.Application.Models.Order;
using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IOrderRepository
{
    Task CreateAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    public Task<bool> ExistsByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task UpdateAsync(OrderUpdateRequestModel orderUpdateRequestModel, CancellationToken cancellationToken);
    Task<int> GetCountAsync(OrderSearchRequestModel searchRequestModel, CancellationToken cancellationToken);
    Task<IEnumerable<Order>> GetAsync(OrderSearchRequestModel searchRequestModel, int page, int size, CancellationToken cancellationToken);
}