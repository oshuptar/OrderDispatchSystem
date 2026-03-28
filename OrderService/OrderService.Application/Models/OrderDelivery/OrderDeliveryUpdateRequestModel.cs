using OrderService.Application.Features.Address.Update.Contracts;

namespace OrderService.Application.Models.OrderDelivery;

public record OrderDeliveryUpdateRequestModel(Guid OrderDeliveryId,
    DateTime? RequestedDeliveryDateTime = null,
    Guid? DestinationAddressId = null);
    