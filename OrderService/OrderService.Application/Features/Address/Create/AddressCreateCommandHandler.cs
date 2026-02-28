using Auth.Mediator.Interfaces;
using Microsoft.Extensions.Logging;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Mappers;
using OrderService.Domain.Models.Extensions;

namespace OrderService.Application.Features.Address.Create;

// TODO: can reuse, so that it accepts address instead of command and controller maps AddressCreateRequest to Address?
public class AddressCreateCommandHandler(
    IAddressRepository repository,
    ILogger<AddressCreateCommandHandler> logger
    ) : ICommandHandler<AddressCreateRequest, AddressCreateResponse>
{
    public async Task<AddressCreateResponse> HandleCommandAsync(AddressCreateRequest command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[{dateTime}]: Registering a new address: {country}, {region}, {city}, {street}, {apartment}, {postalCode}, {longitude}, {latitude}",
            DateTime.Now, command.Country, command.Region, command.City, command.Street ?? String.Empty,
            command.Apartment ?? String.Empty, command.PostalCode ?? String.Empty,
            command.Longitude ?? String.Empty, command.Latitude ?? String.Empty);
        
        var address = command.ToDomainModel();
        var normalisedDeliveryAddress = address.CheckAddressValidity();
        await repository.CreateAddressAsync(normalisedDeliveryAddress, cancellationToken);
        
        logger.LogInformation("[{dateTime}]: Registered a new address: {country}, {region}, {city}, {street}, {apartment}, {postalCode}, {longitude}, {latitude}",
            DateTime.Now, command.Country, command.Region, command.City, command.Street ?? String.Empty,
            command.Apartment ?? String.Empty, command.PostalCode ?? String.Empty,
            command.Longitude ?? String.Empty, command.Latitude ?? String.Empty);
            
        return new AddressCreateResponse(normalisedDeliveryAddress.Id);
    }
}