namespace OrderService.Application.Features.Address.Update.Contracts;

public record AddressUpdateRequest(
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