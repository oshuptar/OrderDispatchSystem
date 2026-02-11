namespace UserService.Application.Features.Admin.Users.Get.Contracts;

public record UserSearchRequest(String? Email, String? Role, int Page, int Size);