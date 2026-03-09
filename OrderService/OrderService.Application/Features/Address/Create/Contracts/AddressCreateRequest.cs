namespace OrderService.Application.Features.Address.Create.Contracts;

public record AddressCreateRequest(
    string Country,
    string Region,
    string City,
    string? Street,
    string? Apartment,
    string? PostalCode,
    string? Longitude,
    string? Latitude
    );