using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Mediator.Interfaces;
using UserService.Application.Models;
using UserService.Application.Models.Mappers;
using UserService.Application.Models.Response;
using UserService.Application.Repositories.Interfaces;
using UserService.Application.UnitOfWork.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Mediator.CommandHandlers;

public class UserRegisterCommandHandler(UserManager<User> userManager,
                                        IUnitOfWork unitOfWork,
                                        ILogger<UserRegisterCommandHandler> logger,
                                        IUserProfileRepository userProfileRepository
    ) : ICommandHandler<UserRegisterRequestDto, UserRegisterResponseDto>
{

    // Creating user and profile is atomic
    public async Task<UserRegisterResponseDto> HandleCommandAsync(UserRegisterRequestDto command,
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
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                String errors = String.Join(separator: ", ", result.Errors.Select(e => e.Description));
                logger.LogError(errors);

                // Do I have to map errors to custom exceptions here?
                if (result.Errors.Any(error =>
                        error.Code == nameof(IdentityErrorDescriber.ConcurrencyFailure)
                        || error.Code == nameof(IdentityErrorDescriber.DefaultError)))
                    throw new Exception("An error occured while registering user");

                throw new Exception($"An error occured while registering user: ${errors}");
            }
            profile.UserId = user.Id;
            user.UserProfile = profile;
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