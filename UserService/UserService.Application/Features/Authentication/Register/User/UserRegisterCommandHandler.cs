using Auth.Abstractions.Persistence;
using Auth.Mediator.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Features.Admin.Users.Update.Contracts;
using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Application.Mappers;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Register.User;

public class UserRegisterCommandHandler(
    UserManager<Domain.Entities.User> userManager,
    IUserProfileRepository userProfileRepository,
    ICommandHandler<UserAddToRoleRequest> addToRoleHandler,
    IUnitOfWork unitOfWork,
    ILogger<UserRegisterCommandHandler> logger
    ) : ICommandHandler<UserRegisterRequest, UserRegisterResponse>
{
    // Creating user, profile and assigning role is atomic
    public async Task<UserRegisterResponse> HandleCommandAsync(UserRegisterRequest command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user {Email}", command.Email);
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Domain.Entities.User user = UserMapper.ToEntity(command);
            UserProfile profile = UserProfileMapper.ToEntity(command);
            var result = await userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                String errors = String.Join(separator: ", ", result.Errors.Select(e => e.Description));
                logger.LogError(errors);

                if (result.Errors.Any(error =>
                        error.Code == nameof(IdentityErrorDescriber.ConcurrencyFailure)
                        || error.Code == nameof(IdentityErrorDescriber.DefaultError)))
                    throw new Exception("An error occured while creating user");

                throw new Exception($"An error occured while creating user: ${errors}");
            }
            profile.UserId = user.Id;
            user.UserProfile = profile;
            await addToRoleHandler.HandleCommandAsync(new UserAddToRoleRequest(user.Id, command.Role), cancellationToken);
            await userProfileRepository.CreateUserProfileAsync(profile, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation("Creating user {Email} succeeded", command.Email);
            return user.ToResponse();
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Creating user {Email} cancelled", command.Email);
            await unitOfWork.RollbackTransactionAsync(transaction, CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("Creating user {Email} failed: {message}", command.Email, ex.Message);
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }
}