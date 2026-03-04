using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Application.Models;

namespace OrderService.Application.Features.Address.Get;

public class AddressSearchByQueryHandler(
    IAddressRepository addressRepository
    ) : IQueryHandler<AddressSearchRequest, AddressSearchResponse>
{
    public async Task<AddressSearchResponse> HandleQueryAsync(AddressSearchRequest command, CancellationToken cancellationToken)
    {
        // TODO: Validate and normalise before comparison
        var requestModel = new AddressSearchRequestModel(command.Country,command.Region,command.City, command.Street,
            command.Apartment,command.PostalCode, command.Longitude, command.Latitude, command.Page, command.Size);
        int totalCount = await addressRepository.GetAddressesCountAsync(requestModel, cancellationToken);
        IReadOnlyCollection<Domain.Models.Address> res = await addressRepository.GetAddressesAsync(
            requestModel,
            cancellationToken);
        return new AddressSearchResponse(res, totalCount);
    }
}