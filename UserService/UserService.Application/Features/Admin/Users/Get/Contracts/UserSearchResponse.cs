using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Users.Get.Contracts;

public record UserSearchResponse(IEnumerable<UserModel> Users, int TotalCount);