using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Repositories;
using OrderService.Application.Models.Address;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;
using OrderService.Infrastructure.Mappers.Mappers;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class AddressRepository(OrderDbContext orderDbContext) : IAddressRepository
{
    // Assumptions is that every address is unique
    public async Task<Address?> GetByIdAsync(Guid addressId, CancellationToken cancellationToken)
    {
        return await orderDbContext.Addresses
            .Where(address => address.Id == addressId)
            .Select(entity => entity.ToDomainModel())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Address>> GetAsync(AddressSearchRequestModel request,
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

    public async Task<int> GetCountAsync(AddressSearchRequestModel request, CancellationToken cancellationToken)
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

    public async Task CreateAsync(Address address, CancellationToken cancellationToken)
    {
        // Move validation here or keep in handlers?
        AddressEntity entity = address.ToEntity();
        await orderDbContext.Addresses.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(AddressUpdateRequestModel addressUpdateRequestModel, CancellationToken cancellationToken)
    {
        AddressEntity? addressEntity = await orderDbContext.Addresses
            .Where(address => address.Id == addressUpdateRequestModel.AddressId)
            .FirstOrDefaultAsync(cancellationToken);
        addressEntity?.Country = addressUpdateRequestModel.Country ?? addressEntity.Country;
        addressEntity?.Region = addressUpdateRequestModel.Region ?? addressEntity.Region;
        addressEntity?.City = addressUpdateRequestModel.City ?? addressEntity.City;
        addressEntity?.Street = addressUpdateRequestModel.Street ?? addressEntity.Street;
        addressEntity?.Apartment = addressUpdateRequestModel.Apartment ?? addressEntity.Apartment;
        addressEntity?.PostalCode = addressUpdateRequestModel.PostalCode ?? addressEntity.PostalCode;
        addressEntity?.Longitude = addressUpdateRequestModel.Longitude ?? addressEntity.Longitude;
        addressEntity?.Latitude = addressUpdateRequestModel.Latitude ?? addressEntity.Latitude;
    }
}