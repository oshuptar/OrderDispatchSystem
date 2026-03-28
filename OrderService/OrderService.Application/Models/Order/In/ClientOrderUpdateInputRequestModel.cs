using OrderService.Application.Features.OrderPlatform.UpdateOrder.Contracts;

namespace OrderService.Application.Models.Order.In;

public record ClientOrderUpdateInputRequestModel(
    int? Volume = null,
    int? Weight = null,
    DateTime? RequestedDeliveryDateTime = null,
    ClientAddressUpdateRequest? DestionationAddressUpdateRequest = null
    );