using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Get.Contracts;

namespace OrderService.Application.Features.Order.Get;

public class OrderGetByIdQueryHandler(
    IOrderRepository orderRepository
    ) : IQueryHandler<OrderGetByIdRequest, Domain.Models.Order>
{
    public async Task<Domain.Models.Order> HandleQueryAsync(OrderGetByIdRequest command, CancellationToken cancellationToken)
    {
        Domain.Models.Order? order = await orderRepository.GetOrderByIdAsync(command.Id, cancellationToken);
        if (order is null)
            throw new NotFoundException($"Order with id {command.Id} not found");
        return order;
    }
}