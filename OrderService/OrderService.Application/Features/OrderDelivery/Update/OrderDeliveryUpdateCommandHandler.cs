using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Application.Features.OrderDelivery.Update.Contracts;
using OrderService.Application.Models.OrderDelivery;

namespace OrderService.Application.Features.OrderDelivery.Update;

public class OrderDeliveryUpdateCommandHandler(
    IQueryHandler<AddressSearchRequest, AddressSearchResponse> addressSearchQueryHandler,
    ICommandHandler<AddressCreateRequest, AddressCreateResponse> addressCreateCommandHandler,
    IOrderDeliveryRepository orderDeliveryRepository,
    IProductionPlantRepository productionPlantRepository
    ) : ICommandHandler<OrderDeliveryUpdateRequest>
{
    public async Task HandleCommandAsync(OrderDeliveryUpdateRequest command, CancellationToken cancellationToken)
    {
        // this would load destinationAddress
        Domain.Models.OrderDelivery? orderDelivery =
            await orderDeliveryRepository.GetByOrderIdAsync(command.OrderId, cancellationToken);
        
        if(orderDelivery is null)
            throw new NotFoundException($"OrderDelivery with id {command.OrderId} not found");
        
        if(command.SourceAddressId.HasValue && !(await productionPlantRepository.ExistsByIdAsync(command.SourceAddressId.Value, cancellationToken)))
           throw new NotFoundException($"Production plant with id {command.SourceAddressId} not found");

        var parametrisedAddress = new Domain.Models.Address
        {
            Country = command.DestionationAddressUpdateRequest?.Country ?? orderDelivery.DestinationAddress!.Country,
            Region = command.DestionationAddressUpdateRequest?.Region ?? orderDelivery.DestinationAddress!.Region,
            City = command.DestionationAddressUpdateRequest?.City ?? orderDelivery.DestinationAddress!.City,
            Street = command.DestionationAddressUpdateRequest?.Street ?? orderDelivery.DestinationAddress!.Street,
            Apartment = command.DestionationAddressUpdateRequest?.Apartment ?? orderDelivery.DestinationAddress!.Apartment,
            PostalCode = command.DestionationAddressUpdateRequest?.PostalCode ?? orderDelivery.DestinationAddress!.PostalCode,
            Longitude = command.DestionationAddressUpdateRequest?.Longitude ?? orderDelivery.DestinationAddress!.Longitude,
            Latitude = command.DestionationAddressUpdateRequest?.Latitude ?? orderDelivery.DestinationAddress!.Latitude,
        };
        
        // if an invalid address was provided - search would fail and afterwards the create would return an exception
        if (command.DestionationAddressUpdateRequest is not null)
        {
            AddressSearchResponse searchResponse = await addressSearchQueryHandler.HandleQueryAsync(
                new AddressSearchRequest(
                parametrisedAddress.Country, parametrisedAddress.Region, parametrisedAddress.City,
                parametrisedAddress.Street, parametrisedAddress.Apartment, parametrisedAddress.PostalCode,
                parametrisedAddress.Longitude, parametrisedAddress.Latitude,
                Page: 0, Size: 1), cancellationToken);
            var address = searchResponse.Addresses.FirstOrDefault();
            if (address is null)
            {
                AddressCreateResponse createResponse = await addressCreateCommandHandler.HandleCommandAsync(
                    new AddressCreateRequest(
                        parametrisedAddress.Country, parametrisedAddress.Region, parametrisedAddress.City,
                        parametrisedAddress.Street, parametrisedAddress.Apartment, parametrisedAddress.PostalCode,
                        parametrisedAddress.Longitude, parametrisedAddress.Latitude
                    ), cancellationToken);
                orderDelivery.DestinationAddressId = createResponse.Id;
            }
            else
                orderDelivery.DestinationAddressId = address.Id;
        }
        await orderDeliveryRepository.UpdateAsync(
            new OrderDeliveryUpdateRequestModel(
            orderDelivery.Id,
            orderDelivery.ScheduledDeliveryDateTime,
            orderDelivery.DestinationAddressId,
            orderDelivery.OrderDeliveryStatus,
            command.SourceAddressId
            ), cancellationToken);
    }
}