namespace OrderService.Application.Features.Address.Get.Contracts;

// TODO: Normalise fields before saving
public record AddressSearchRequest
(
    string? Country,
    string? Region,
    string? City,
    string? Street,
    string? Apartment,
    string? PostalCode,
    string? Longitude,
    string? Latitude,
    int Page = 0,
    int Size = 5
);