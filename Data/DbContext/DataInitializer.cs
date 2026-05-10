using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DbContext
{
    public class DataInitializer(ApplicationDbContexts dbContext, UserManager<User> userManager)
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
          if (await dbContext.Ads.AnyAsync()) return;


            var seller = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "Seller1@seller.com");
            var customer = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "Customer1@customer.com");
           if (seller == null || customer == null)
            {
                throw new Exception("Seller or Customer user not found. Ensure users are seeded before ads.");
            }
            var ad1 = new Ad
            {
                Title = "iPhone 12",
                Description = "A barely used iPhone 12 in excellent condition.",
                StartingPrice = 500,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Place ="Stockholm",
                SellerId = seller.Id,

                Bids = new List<Bid>
                {
                    new Bid
                    {
                       BidAmount = 550,
                          BidDate = DateTime.UtcNow.AddDays(1),
                          UserId = customer.Id
                         
                         
                    }
                },

                Images = new List<Images>
                {
                    new Images
                    {
                       Url = "https://example.com/iphone12.jpg",
                          Description = "Front view of the iPhone 12",
                        

                    }
                }






            };

            await dbContext.Ads.AddAsync(ad1);

            await dbContext.SaveChangesAsync();








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
