namespace UserService.Application.Models;

public record UserSearchRequestModel(String? Email, String? Role, int Page, int Size);