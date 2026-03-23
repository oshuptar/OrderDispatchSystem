namespace OrderService.Application.Models.Address;

public record AddressSearchRequestModel
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
    int Size = 1
);