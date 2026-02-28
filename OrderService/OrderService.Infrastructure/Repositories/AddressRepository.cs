using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Models;
using OrderService.Domain.Models;
using OrderService.Domain.Models.Extensions;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class AddressRepository(OrderDbContext orderDbContext) : IAddressRepository
{
    // Assumptions is that every address is unique
    public async Task<IReadOnlyCollection<Address>> GetAddressesAsync(AddressSearchRequestModel request,
        CancellationToken cancellationToken)
    {
        return await orderDbContext.Addresses
            .AsNoTracking()
            .Where(a =>
                (request.Country == null || a.Country == request.Country) &&
                (request.Region == null || a.Region == request.Region) &&
                (request.City == null || a.City == request.City) &&
                (request.Street == null || a.Street == request.Street) &&
                (request.Apartment == null || a.Apartment == request.Apartment) &&
                (request.PostalCode == null || a.PostalCode == request.PostalCode) &&
                (request.Longitude == null || a.Longitude == request.Longitude) &&
                (request.Latitude == null || a.Latitude == request.Latitude)
            )
            .Select(entity => entity.ToDomainModel())
            .ToListAsync<Address>(cancellationToken);
    }

    public async Task<int> GetAddressesCountAsync(AddressSearchRequestModel request, CancellationToken cancellationToken)
    {
        return await orderDbContext.Addresses
            .AsNoTracking()
            .Where(a =>
                (request.Country == null || a.Country == request.Country) &&
                (request.Region == null || a.Region == request.Region) &&
                (request.City == null || a.City == request.City) &&
                (request.Street == null || a.Street == request.Street) &&
                (request.Apartment == null || a.Apartment == request.Apartment) &&
                (request.PostalCode == null || a.PostalCode == request.PostalCode) &&
                (request.Longitude == null || a.Longitude == request.Longitude) &&
                (request.Latitude == null || a.Latitude == request.Latitude)
            ).CountAsync(cancellationToken);
    }

    public async Task CreateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        var normalisedAddress = address.CheckAddressValidity();
        AddressEntity entity = normalisedAddress.ToEntity();
        await orderDbContext.Addresses.AddAsync(entity, cancellationToken);
    }
}