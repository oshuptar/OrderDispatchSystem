using UserService.Application.Models.Response;
using UserService.Domain.Entities;

namespace UserService.Application.Models.Mappers;

public static class UserMapper
{
    // In mappings:
    public static User ToEntity(this UserRegisterRequestDto dto)
    {
        return new User()
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            UserName = dto.Email
        };
    }
    
    // Out mappings:
    public static UserRegisterResponseDto ToResponse(this User user)
    {
        // References Microsoft.Extensions.Identity.Stores - to silence the error
        return new UserRegisterResponseDto(user.Id);
    }
}