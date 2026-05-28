using Data.DbContext;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContexts _dbContext;
        public readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContexts dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<User> CreateAsync(User user)
        {
          var result = await _userManager.CreateAsync(user);
            if(result.Succeeded)
            {
                return user;
            }
            else
            {
                throw new Exception("Couldnt create user");
            }
        }

        public async Task<IEnumerable<User>> GetAllCustomersAsync()
        { 
            var Customer = await _userManager.GetUsersInRoleAsync("Customer");
            return Customer;
        }

        public  async Task<IEnumerable<User>> GetAllSellersAsync()
        {
            var Seller = await _userManager.GetUsersInRoleAsync("Seller");
            return Seller;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
