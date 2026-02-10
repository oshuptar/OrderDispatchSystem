namespace UserService.Application.Features.Admin.Users.Update.Contracts;

public record UserAddToRoleRequest(Guid Id, String Role);