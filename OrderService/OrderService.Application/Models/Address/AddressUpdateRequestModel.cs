namespace OrderService.Application.Models.Address;

public record AddressUpdateRequestModel(
    Guid AddressId,
    string? Country = null,
    string? Region = null,
    string? City = null,
    string? Street = null,
    string? Apartment = null,
    string? PostalCode = null,
    string? Longitude = null,
    string? Latitude = null
);