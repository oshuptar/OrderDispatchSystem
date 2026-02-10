using UserService.Application.Features.Authentication.Register.Contracts;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Mappers;

public record UserModelContext(IEnumerable<String> Roles);

public static class UserMapper
{
    // In mappings:
    public static User ToEntity(this UserRegisterRequest model)
    {
        return new User()
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            UserName = model.Email
        };
    }
    
    // Out mappings:
    public static UserRegisterResponse ToResponse(this User user)
    {
        // References Microsoft.Extensions.Identity.Stores - to silence the error
        return new UserRegisterResponse(user.Id);
    }

    public static UserModel ToUserModel(this User user, UserModelContext context)
    {
        return new UserModel(user.Id,
            user.Email!,
            user.UserProfile!.FirstName,
            user.UserProfile!.LastName,
            user.PhoneNumber ?? string.Empty,
            context.Roles);
    }
}
