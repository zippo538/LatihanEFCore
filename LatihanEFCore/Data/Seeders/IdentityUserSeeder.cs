using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LatihanEFCore.Data.Seeders
{
    public static class IdentityUserSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
            "Admin",
            "User"
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Failed to create role {role}: {errors}");
                    }
                }
            }

            await CreateUserAsync(
                userManager,
                name: "Administrator",
                email: "admin@example.com",
                password: "Admin123!",
                role: "Admin");

            await CreateUserAsync(
                userManager,
                name: "Default User",
                email: "user@example.com",
                password: "User123!",
                role: "User");
        }

        private static async Task CreateUserAsync(
            UserManager<ApplicationUser> userManager,
            string name,
            string email,
            string password,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    var addRoleResult =
                        await userManager.AddToRoleAsync(existingUser, role);

                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            addRoleResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Failed to assign role {role}: {errors}");
                    }
                }

                return;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult =
                await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create user {email}: {errors}");
            }

            var roleResult =
                await userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to assign role {role}: {errors}");
            }
        }
    }
}