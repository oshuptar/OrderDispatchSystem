using UserService.Application.Features.Authentication.Register.Customer.Contracts;
using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Domain.Constants;

namespace UserService.Application.Common.Mappers;

public static class UserRegisterMapper
{
    public static UserRegisterRequest ToUserRegisterRequest(this CustomerRegisterRequest command)
    {
        return new(command.FirstName,
            command.LastName,
            command.Email,
            command.Password,
            command.PhoneNumber,
            Roles.Customer);
    }
}