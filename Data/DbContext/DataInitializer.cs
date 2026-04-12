using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DbContext
{
    public class DataInitializer(ApplicationDbContext dbContext, UserManager<User> userManager)
    {
        /// <summary>
        /// Seeds initial data asynchronously.
        /// </summary>
        public async Task SeedData()
        {
            await dbContext.Database.MigrateAsync();

            await SeedRoles();
            await SeedUsers();
            await SeedAds();

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Seeds initial users for the application.
        /// </summary>
        private async Task SeedUsers()
        {
            await AddUserIfNotExists("Admin1@admin.com", "Erik", "Lund", "Lundgatan 2", "Stockholm", "Sweden", "Hejsan123#", ["Admin"]);
            await AddUserIfNotExists("Seller1@seller.com", "Amanda", "Persson", "Lundgatan 29", "Stockholm", "Sweden", "Hejsan123#", ["Seller"]);
            await AddUserIfNotExists("Customer1@customer.com", "Gunila", "Andersson", "Lundgatan 3", "Stockholm", "Sweden", "Hejsan123#", ["Customer"]);
        }

        /// <summary>
        /// Seeds initial roles for the application.
        /// </summary>
        private async Task SeedRoles()
        {
            await AddRoleIfNotExisting("Admin");
            await AddRoleIfNotExisting("Seller");
            await AddRoleIfNotExisting("Customer");
        }

        /// <summary>
        /// Seeds initial sessions for the application.
        /// </summary>
        /// <returns></returns>
        private async Task SeedAds()
        {
          
        }

        private async Task AddRoleIfNotExisting(string roleName)
        {
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role != null)
            {
                return;
            }

            role = new IdentityRole<int> { Name = roleName, NormalizedName = roleName };
            await dbContext.Roles.AddAsync(role);
        }

        /// <summary>
        /// Adds a user by username if it doesn't already exist, along with password and roles.
        /// </summary>
        /// <param name="userName">The username to set. Is be used to identify the user.</param>
        /// <param name="password">The password to set. Will be hashed.</param>
        /// <param name="roles">Roles to give this user.</param>
        private async Task AddUserIfNotExists(string userName, string firstName, string lastName, string address, string city, string country, string password, string[] roles)
        {
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return;
            }
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                City = city,
                Country = country,
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRolesAsync(user, roles);
        }

    }
}
