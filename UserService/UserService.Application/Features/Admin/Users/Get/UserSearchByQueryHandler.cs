using Auth.Exceptions;
using Auth.Mediator.Interfaces;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Features.Admin.Users.Get.Contracts;
using UserService.Application.Mappers;
using UserService.Application.Models;
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
        
        int totalCount = await userRepository.GetUsersCountAsync(command, cancellationToken);
        IReadOnlyCollection<User> res = await userRepository.GetUsersAsync(
            new UserSearchRequestModel(command.Email, command.Role, command.Page, command.Size),
            cancellationToken);
        return new UserSearchResponse(res.ToUserModelList(), totalCount);
    }
}