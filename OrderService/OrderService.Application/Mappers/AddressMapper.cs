using OrderService.Application.Features.Address.Create.Contracts;
using OrderService.Application.Features.Address.Get.Contracts;
using OrderService.Domain.Models;

namespace OrderService.Application.Mappers;

public static class AddressMapper
{
    public static Address ToDomainModel(this AddressCreateRequest request)
    {
        return new Address()
        {
            Country = request.Country,
            Region = request.Region,
            City = request.City,
            Street = request.Street,
            Apartment = request.Apartment,
            PostalCode = request.PostalCode,
            Longitude = request.Longitude,
            Latitude = request.Latitude
        };
    }
}