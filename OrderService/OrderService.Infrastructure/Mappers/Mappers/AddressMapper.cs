using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers.Mappers;

public static class AddressMapper
{
    public static Address ToDomainModel(this AddressEntity entity)
    {
        return new Address
        {
            Id = entity.Id,
            City = entity.City,
            Country = entity.Country,
            PostalCode = entity.PostalCode,
            Street = entity.Street,
            Region = entity.Region,
            Apartment = entity.Apartment,
            Longitude = entity.Longitude,
            Latitude = entity.Latitude
        };
    }

    public static AddressEntity ToEntity(this Address model)
    {
        return new AddressEntity
        {
            Id = model.Id,
            Country = model.Country,
            Region = model.Region,
            City = model.City,
            Street = model.Street,
            Apartment = model.Apartment,
            PostalCode = model.PostalCode,
            Longitude = model.Longitude,
            Latitude = model.Latitude
        };
    }
}