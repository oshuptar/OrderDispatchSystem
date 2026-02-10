using Microsoft.AspNetCore.Identity;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        String[] roleNames = [Roles.Customer, Roles.Admin, Roles.Driver];
        foreach (var roleName in roleNames)
        {
            if(await roleManager.RoleExistsAsync(roleName)) continue;
            Role role = new Role() { Name = roleName };
            await roleManager.CreateAsync(role);
        }
    }
}