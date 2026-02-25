using UserService.Application.Features.Authentication.Register.Customer.Contracts;
using UserService.Application.Features.Authentication.Register.User.Contracts;
using UserService.Application.Mappers;
using UserService.Application.Mediator.Interfaces;

namespace UserService.Application.Features.Authentication.Register.Customer;

public class CustomerRegisterCommandHandler(
    ICommandHandler<UserRegisterRequest, UserRegisterResponse> userRegisterHandler
    ) : ICommandHandler<CustomerRegisterRequest, CustomerRegisterResponse>
{
    
    public async Task<CustomerRegisterResponse> HandleCommandAsync(CustomerRegisterRequest command,
        CancellationToken cancellationToken)
    {
        UserRegisterRequest registerCommand = command.ToUserRegisterRequest();
        UserRegisterResponse res = await userRegisterHandler.HandleCommandAsync(registerCommand, cancellationToken);
        return new CustomerRegisterResponse(res.Id);
    }
}