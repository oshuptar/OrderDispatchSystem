namespace UserService.Application.Features.Authentication.Register.Customer.Contracts;

public record CustomerRegisterRequest(String FirstName,
    String LastName,
    String Email,
    String Password,
    String PhoneNumber);