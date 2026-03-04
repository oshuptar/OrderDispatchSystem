namespace OrderService.Application.Features.Address.Get.Contracts;

// TODO: Normalise fields before saving
public record AddressSearchRequest
(
    string? Country = null,
    string? Region = null,
    string? City = null,
    string? Street = null,
    string? Apartment = null,
    string? PostalCode = null,
    string? Longitude = null,
    string? Latitude = null,
    int Page = 0,
    int Size = 5
);