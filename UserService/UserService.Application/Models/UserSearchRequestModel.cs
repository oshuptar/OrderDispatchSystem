namespace UserService.Application.Models;

public record UserSearchRequestModel(String? Email, String? Role, int Page = 0, int Size = 10);