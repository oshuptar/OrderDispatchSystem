using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Mappers;

public record UserModelContext(IEnumerable<String> Roles);

public static class UserMapper
{
    // In mappings:
    public static User ToEntity(this UserRegisterRequest command)
    {
        return new User()
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            UserName = command.Email
        };
    }
    
    // Out mappings:
    public static UserRegisterResponse ToResponse(this User user)
    {
        // References Microsoft.Extensions.Identity.Stores - to silence the error
        return new UserRegisterResponse(user.Id);
    }

    public static UserDetailsModel ToUserDetailsModel(this User user, UserModelContext context)
    {
        return new UserDetailsModel(user.Id,
            user.Email!,
            user.UserProfile!.FirstName,
            user.UserProfile!.LastName,
            user.PhoneNumber ?? string.Empty,
            context.Roles);
    }

    public static UserModel ToUserModel(this User user)
    {
        return new UserModel(user.Id,
            user.Email!,
            user.UserProfile!.FirstName,
            user.UserProfile!.LastName);
    }

    public static IEnumerable<UserModel> ToUserModelList(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToUserModel());
    }
}
