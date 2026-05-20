namespace OrderService.Application.Features.Address.Get.Contracts;
using Domain.Models;

public record AddressSearchResponse(IEnumerable<Address> Addresses, int TotalCount);