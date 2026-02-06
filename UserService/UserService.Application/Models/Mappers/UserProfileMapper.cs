using UserService.Domain.Entities;

namespace UserService.Application.Models.Mappers;

public static class UserProfileMapper
{
    // More fields to be added
    public static UserProfile ToEntity(this UserRegisterRequestDto dto)
    {
        return new UserProfile()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }
}