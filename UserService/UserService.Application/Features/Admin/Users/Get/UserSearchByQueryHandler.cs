using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Common.Exceptions;
using UserService.Application.Common.Mappers;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Users.Get;

public class UserSearchByQueryHandler(
    IUserRepository userRepository
    ) : IQueryHandler<UserSearchRequest, UserSearchResponse>
{
    public async Task<UserSearchResponse> HandleQueryAsync(UserSearchRequest command, CancellationToken cancellationToken)
    {
        if (command.Page <= 0)
            throw new BadRequestException("Page must be positive");
        if(command.Size <= 0)
            throw new BadRequestException("Size must be positive");

        IQueryable<User> users = command.Role is not null && command.Role != String.Empty
                ? userRepository.GetUsersInRole(command.Role)
                : userRepository.GetAllUsers();

        users = users.Where(user =>
            command.Email == null || user.Email == command.Email);

        int totalCount = await users.CountAsync(cancellationToken);
        IReadOnlyCollection<User> res = await users
            .Include(user => user.UserProfile)
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((command.Page - 1) * command.Size)
            .Take(command.Size)
            .ToListAsync(cancellationToken);

        return new UserSearchResponse(res.ToUserModelList(), totalCount);
    }
}