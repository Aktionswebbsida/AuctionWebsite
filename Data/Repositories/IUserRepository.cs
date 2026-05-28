using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);
        Task<User?> GetUserByIdAsync(int id);

        Task<IEnumerable<User>> GetAllSellersAsync();

        Task<IEnumerable<User>> GetAllCustomersAsync();

        public Task SaveChangesAsync();
    }
}
