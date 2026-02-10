using System.Linq.Expressions;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions.Repositories;

public interface IUserRepository
{
    IQueryable<User> GetUserByIdAsync(Guid userId);
    
    IQueryable<User> GetUsersInRole(String role);
    
    IQueryable<User> GetAllUsers();
}