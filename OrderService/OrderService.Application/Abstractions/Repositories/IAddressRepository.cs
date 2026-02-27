using OrderService.Application.Models;
using OrderService.Domain.Models;

namespace OrderService.Application.Abstractions.Repositories;

public interface IAddressRepository
{
   Task<IReadOnlyCollection<Address>> GetAddressesAsync(AddressSearchRequestModel request, CancellationToken cancellationToken);
   Task<int> GetAddressesCountAsync(AddressSearchRequestModel request, CancellationToken cancellationToken);
}