namespace UserService.Application.Models;

public record UserModel (
    Guid Id,
    String Email,
    String FirstName,
    String LastName,
    String PhoneNumber,
    IEnumerable<String> Role);