namespace UserService.Application.Features.Authentication.Register.Contracts;

public record UserRegisterRequest(String FirstName,
    String LastName,
    String Email,
    String Password,
    String PhoneNumber);