using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RegistroDoPonto.Models;

namespace RegistroDoPonto.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Criar usuário administrador se não existir
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    Nome = "Administrador",
                    EmailConfirmed = true,
                    IsAtivo = true // Garante que o admin seja criado como ativo
                };
                var createAdmin = await userManager.CreateAsync(adminUser, "Admin@123"); // Senha forte
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }

    public static async Task ActivateExistingUsers(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var usersToUpdate = await userManager.Users.Where(u => !u.IsAtivo).ToListAsync();

            foreach (var user in usersToUpdate)
            {
                user.IsAtivo = true;
                await userManager.UpdateAsync(user);
            }
        }
    }
}
