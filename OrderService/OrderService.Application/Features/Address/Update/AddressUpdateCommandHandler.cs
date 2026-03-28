using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Application.Features.Address.Update.Contracts;
using OrderService.Application.Models.Address;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.Address.Update;

public class AddressUpdateCommandHandler(
    IQueryHandler<AddressGetByIdRequest, Domain.Models.Address> addressGetByIdQueryHandler,
    IAddressRepository addressRepository
    ) : ICommandHandler<AddressUpdateRequest>
{
    public async Task HandleCommandAsync(AddressUpdateRequest command, CancellationToken cancellationToken)
    {
        Domain.Models.Address address = await addressGetByIdQueryHandler.HandleQueryAsync(
            new AddressGetByIdRequest(command.AddressId), cancellationToken);
        address.Country = command.Country ?? address.Country;
        address.City = command.City ?? address.City;
        address.Street = command.Street ?? address.Street;
        address.Apartment = command.Apartment ?? address.Apartment;
        address.PostalCode = command.PostalCode ??  address.PostalCode;
        address.Latitude = command.Latitude ?? address.Latitude;
        address.Longitude = command.Longitude ?? address.Longitude;
        Domain.Models.Address normalisedAddress = address.CheckAddressValidity();
        
        await addressRepository.UpdateAsync(new AddressUpdateRequestModel(
            normalisedAddress.Id, normalisedAddress.Country,
            normalisedAddress.City, normalisedAddress.Street,
            normalisedAddress.Apartment, normalisedAddress.PostalCode,
            normalisedAddress.Latitude, normalisedAddress.Longitude
            ), cancellationToken);
    }
}