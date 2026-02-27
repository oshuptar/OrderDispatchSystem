namespace OrderService.Application.Models;

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
    int Page,
    int Size
);