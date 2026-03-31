using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Order.Get.Contracts;
using OrderService.Application.Models.Order;

namespace OrderService.Application.Features.Order.Get;

public class OrderSearchByQueryHandler(
IOrderRepository orderRepository
) : IQueryHandler<OrderSearchByRequest, OrderSearchByResponse>
{
    public async Task<OrderSearchByResponse> HandleQueryAsync(OrderSearchByRequest command, CancellationToken cancellationToken)
    {
        if (command.Page <= 0)
            throw new BadRequestException("Page must be positive");
        if(command.Size <= 0)
            throw new BadRequestException("Size must be positive");

        OrderSearchRequestModel requestModel = new OrderSearchRequestModel(
            command.ProductionPlantId,
            command.OrderStatus,
            command.UserId,
            command.StartDeliveryDateTime,
            command.EndDeliveryDateTime
        );
        int totalCount = await orderRepository.GetCountAsync(requestModel, cancellationToken);
        var orders = await orderRepository.GetAsync(requestModel, command.Page, command.Size, cancellationToken);
        return new OrderSearchByResponse(orders, totalCount);
    }
}