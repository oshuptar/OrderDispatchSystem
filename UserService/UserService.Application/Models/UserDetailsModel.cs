namespace UserService.Application.Models;

public record UserDetailsModel (
    Guid Id,
    String Email,
    String FirstName,
    String LastName,
    String PhoneNumber,
    IEnumerable<String> Roles);