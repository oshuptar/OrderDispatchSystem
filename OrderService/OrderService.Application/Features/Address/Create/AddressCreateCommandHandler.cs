using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Mappers;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.Address.Create;

public class AddressCreateCommandHandler(
    IAddressRepository repository
    ) : ICommandHandler<AddressCreateRequest, AddressCreateResponse>
{
    public async Task<AddressCreateResponse> HandleCommandAsync(AddressCreateRequest command, CancellationToken cancellationToken)
    {
        var address = command.ToDomainModel();
        var normalisedDeliveryAddress = address.CheckAddressValidity();
        await repository.CreateAddressAsync(normalisedDeliveryAddress, cancellationToken);
        return new AddressCreateResponse(normalisedDeliveryAddress.Id);
    }
}
//
// logger.LogInformation("[{dateTime}]: Registered a new address: {country}, {region}, {city}, {street}, {apartment}, {postalCode}, {longitude}, {latitude}",
//     DateTime.Now, command.Country, command.Region, command.City, command.Street ?? String.Empty,
//     command.Apartment ?? String.Empty, command.PostalCode ?? String.Empty,
//     command.Longitude ?? String.Empty, command.Latitude ?? String.Empty);