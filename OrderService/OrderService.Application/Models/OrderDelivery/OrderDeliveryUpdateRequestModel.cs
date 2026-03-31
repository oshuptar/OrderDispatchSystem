using OrderService.Application.Features.Address.Update.Contracts;
using OrderService.Domain.Enums;

namespace OrderService.Application.Models.OrderDelivery;

public record OrderDeliveryUpdateRequestModel(
    Guid OrderDeliveryId,
    DateTime? RequestedDeliveryDateTime = null,
    Guid? DestinationAddressId = null,
    OrderDeliveryStatus? OrderDeliveryStatus = null,
    Guid? SourceAddressId = null
    );
    