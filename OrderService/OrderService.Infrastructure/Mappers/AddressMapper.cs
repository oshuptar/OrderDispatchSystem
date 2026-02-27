using OrderService.Domain.Models;
using OrderService.Infrastructure.Entities;

namespace OrderService.Infrastructure.Mappers;

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
}