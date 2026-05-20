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
        IQueryable<AddressEntity> query = orderDbContext.Addresses.AsNoTracking();
        if (request.Country != null) query = query.Where(a => a.Country == request.Country);
        if (request.Region != null) query = query.Where(a => a.Region == request.Region);
        if (request.City != null) query = query.Where(a => a.City == request.City);
        if (request.Street != null) query = query.Where(a => a.Street == request.Street);
        if (request.Apartment != null) query = query.Where(a => a.Apartment == request.Apartment);
        if (request.PostalCode != null) query = query.Where(a => a.PostalCode == request.PostalCode);
        if (request.Longitude != null) query = query.Where(a => a.Longitude == request.Longitude);
        if (request.Latitude != null) query = query.Where(a => a.Latitude == request.Latitude);
        return await query.Select(entity => entity.ToDomainModel()).ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(AddressSearchRequestModel request, CancellationToken cancellationToken)
    {
        IQueryable<AddressEntity> query = orderDbContext.Addresses.AsNoTracking();
        if (request.Country != null) query = query.Where(a => a.Country == request.Country);
        if (request.Region != null) query = query.Where(a => a.Region == request.Region);
        if (request.City != null) query = query.Where(a => a.City == request.City);
        if (request.Street != null) query = query.Where(a => a.Street == request.Street);
        if (request.Apartment != null) query = query.Where(a => a.Apartment == request.Apartment);
        if (request.PostalCode != null) query = query.Where(a => a.PostalCode == request.PostalCode);
        if (request.Longitude != null) query = query.Where(a => a.Longitude == request.Longitude);
        if (request.Latitude != null) query = query.Where(a => a.Latitude == request.Latitude);
        return await query.CountAsync(cancellationToken);
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