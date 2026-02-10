using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions.Persistence;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Common.Mappers;
using UserService.Application.Features.Authentication.Register.Contracts;
using UserService.Application.Mediator.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Register;

public class UserRegisterCommandHandler(UserManager<User> userManager,
                                        IUnitOfWork unitOfWork,
                                        IUserProfileRepository userProfileRepository,
                                        ILogger<UserRegisterCommandHandler> logger
    ) : ICommandHandler<UserRegisterRequest, UserRegisterResponse>
{

    // Creating user and profile is atomic
    public async Task<UserRegisterResponse> HandleCommandAsync(UserRegisterRequest command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user {Email}", command.Email);
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            User user = UserMapper.ToEntity(command);
            UserProfile profile = UserProfileMapper.ToEntity(command);
            var result = await userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                String errors = String.Join(separator: ", ", result.Errors.Select(e => e.Description));
                logger.LogError(errors);

                if (result.Errors.Any(error =>
                        error.Code == nameof(IdentityErrorDescriber.ConcurrencyFailure)
                        || error.Code == nameof(IdentityErrorDescriber.DefaultError)))
                    throw new Exception("An error occured while registering user");

                throw new Exception($"An error occured while registering user: ${errors}");
            }
            profile.UserId = user.Id;
            user.UserProfile = profile;
            // Registered users are customers by default, other users can be added by admin
            await userManager.AddToRoleAsync(user, Roles.Customer);
            await userProfileRepository.CreateUserProfileAsync(profile, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            logger.LogInformation("Registering user {Email} succeeded", command.Email);
            return user.ToResponse();
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Registering user {Email} cancelled", command.Email);
            await unitOfWork.RollbackTransactionAsync(transaction, CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("Registering user {Email} failed: {message}", command.Email, ex.Message);
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
    }
}