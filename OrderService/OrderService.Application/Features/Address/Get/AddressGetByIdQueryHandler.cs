using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Features.Address.Get.Contracts;

namespace OrderService.Application.Features.Address.Get;

public class AddressGetByIdQueryHandler
(
    IAddressRepository addressRepository
) : IQueryHandler<AddressGetByIdRequest, Domain.Models.Address>
{
    public async Task<Domain.Models.Address> HandleQueryAsync(AddressGetByIdRequest command, CancellationToken cancellationToken)
    {
        Domain.Models.Address? address = await addressRepository.GetByIdAsync(command.AddressId, cancellationToken);
        if (address is null)
            throw new NotFoundException($"Address with id: {command.AddressId} not found");
        return address;
    }
}