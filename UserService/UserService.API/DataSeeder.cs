using Microsoft.AspNetCore.Identity;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var logger = serviceProvider.GetService(typeof(ILogger<Program>)) as ILogger<Program> 
                     ?? throw new Exception($"[{nameof(DataSeeder)}] - Failed to get logger");
        await SeedRolesAsync(roleManager);
        await SeedAdminUserAsync(userManager, configuration, logger);
    }
    
    private static async Task SeedAdminUserAsync(
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger<Program> logger)
    {
        User adminConf = configuration.GetSection("Admin").Get<User>() 
                    ?? throw new Exception($"[{nameof(DataSeeder)}] - Invalid Admin User");
        User? adminUser = await userManager.FindByEmailAsync(adminConf.Email!);
        if (adminUser is null)
        {
            adminUser = new User()
            {
                Email = adminConf.Email,
                UserName = adminConf.Email,
                UserProfile = adminConf.UserProfile
            };
            var res = await userManager.CreateAsync(adminUser, adminConf.PasswordHash!);
            if(res.Succeeded) 
                logger.LogInformation($"[{nameof(DataSeeder)}] - Admin user created");
            else 
                logger.LogError($"[{nameof(DataSeeder)}] - Failed to create admin user: {res.Errors.First().Description}");
        }
        else
            logger.LogInformation($"[{nameof(DataSeeder)}] - Admin user already exists");
         
        if (!await userManager.IsInRoleAsync(adminUser, Roles.Admin))
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
    
    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        String[] roleNames = [Roles.Customer, Roles.Admin, Roles.Driver];
        foreach (var roleName in roleNames)
        {
            if(await roleManager.RoleExistsAsync(roleName)) continue;
            Role role = new Role() { Name = roleName };
            await roleManager.CreateAsync(role);
        }
    }
}