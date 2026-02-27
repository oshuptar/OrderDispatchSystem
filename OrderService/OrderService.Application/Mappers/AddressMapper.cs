using OrderService.Application.Models;
using OrderService.Domain.Models;

namespace OrderService.Application.Mappers;

public static class AddressMapper
{
    public static Address ToDomainModel(this AddressRequestModel model)
    {
        return new Address()
        {
            Country = model.Country,
            Region = model.Region,
            City = model.City,
            Street = model.Street,
            Apartment = model.Street,
            PostalCode = model.PostalCode,
            Longitude = model.Longitude,
            Latitude = model.Latitude
        };
    }
}