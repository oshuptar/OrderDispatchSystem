using Auth.Exceptions;

namespace OrderService.Domain.Models.Extensions;

// TODO: to finish AddressExtensions
public static class AddressExtensions
{
    public static Address CheckAddressValidity(this Address address)
    {
        Address normalisedAddress = address.NormaliseAddress();
        // checks whether the valid address was provided
        bool addressStructure = normalisedAddress.CheckAddressStructure();
        bool addressFormat = normalisedAddress.CheckAddressFormat();
        bool addressExistence = normalisedAddress.CheckAddressExistence();
        
        // Throws custom exceptions or generic?
        if (!addressStructure)
            throw new BadRequestException("Address structure is invalid");

        if (!addressFormat)
            throw new BadRequestException("Address format is invalid");

        if (!addressExistence)
            throw new BadRequestException("Address does not exist");

        return normalisedAddress;
    }

    private static Address NormaliseAddress(this Address address)
    {
        return new Address()
        {
            Id = address.Id,
            Country = address.Country.TrimStart().TrimEnd(),
            Region = address.Region.TrimStart().TrimEnd(),
            City = address.City.TrimStart().TrimEnd(),
            Street = address.Street?.TrimStart().TrimEnd(),
            Apartment = address.Apartment?.TrimStart().TrimEnd(),
            PostalCode = address.PostalCode?.TrimStart().TrimEnd(),
            Longitude = address.Longitude?.TrimStart().TrimEnd(),
            Latitude = address.Latitude?.TrimStart().TrimEnd()
        };
    }

    private static bool CheckAddressStructure(this Address address)
    {
        return (address.Longitude is not null && address.Latitude is not null) 
               || (address.Street is not null && address.Apartment is not null && address.PostalCode is not null);
    }

    private static bool CheckAddressFormat(this Address address)
    {
        // checks whether the coordinates are provided with the desired format
        return true;
    }

    // Checks the length constraints according to values provided in configuration
    private static bool CheckAddressLengthConstraint(this Address address)
    {
        return true;
    }
    
    // TODO: Shall use Google Api or smth
    private static bool CheckAddressExistence(this Address address)
    {
        // temporarily returned true for testing purposes
        return true;
    }
}