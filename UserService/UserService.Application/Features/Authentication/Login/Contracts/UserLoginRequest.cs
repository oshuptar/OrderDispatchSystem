namespace UserService.Application.Features.Authentication.Login.Contracts;

public record UserLoginRequest(String Email, String Password);