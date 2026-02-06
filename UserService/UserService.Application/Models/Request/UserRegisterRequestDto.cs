namespace UserService.Application.Models;

public record UserRegisterRequestDto(String FirstName,
    String LastName,
    String Email,
    String Password,
    String PhoneNumber);