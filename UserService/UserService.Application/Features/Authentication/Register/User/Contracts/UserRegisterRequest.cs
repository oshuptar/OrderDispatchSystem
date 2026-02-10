namespace UserService.Application.Features.Authentication.Register.User.Contracts;

public record UserRegisterRequest(
    String FirstName,
    String LastName,
    String Email,
    String Password,
    String PhoneNumber,
    String Role)
{ }