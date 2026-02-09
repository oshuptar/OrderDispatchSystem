using UserService.Application.Features.Authentication.Register.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Mappers;

public static class UserProfileMapper
{
    // More fields to be added
    public static UserProfile ToEntity(this UserRegisterRequest model)
    {
        return new UserProfile()
        {
            FirstName = model.FirstName,
            LastName = model.LastName
        };
    }
}