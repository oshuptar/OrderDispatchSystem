namespace OrderService.Application.Models;

public record AddressRequestModel(
    string Country,
    string Region,
    string City,
    string? Street,
    string? Apartment,
    string? PostalCode,
    string? Longitude,
    string? Latitude);