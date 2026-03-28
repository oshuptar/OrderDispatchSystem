using OrderService.Application.Models.Address;
using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IAddressRepository
{
   Task<Address?> GetByIdAsync(Guid  addressId, CancellationToken cancellationToken);
   Task<IReadOnlyCollection<Address>> GetAsync(AddressSearchRequestModel request, CancellationToken cancellationToken);
   Task<int> GetCountAsync(AddressSearchRequestModel request, CancellationToken cancellationToken);
   Task CreateAsync(Address address, CancellationToken cancellationToken);
   Task UpdateAsync(AddressUpdateRequestModel addressUpdateRequestModel, CancellationToken cancellationToken);
}