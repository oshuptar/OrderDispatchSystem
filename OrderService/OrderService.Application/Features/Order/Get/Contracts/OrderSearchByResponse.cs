using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Order.Get.Contracts;

public record OrderSearchByResponse(
    IEnumerable<Domain.Models.Order> Orders,
    int TotalCount
    );