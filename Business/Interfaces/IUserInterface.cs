using Data.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IUserInterface
    {
        Task<UserDTO?> GetUserByIdAsync(int id);

        Task<IEnumerable<UserDTO>> GetAllSellersAsync();

        Task<IEnumerable<UserDTO>> GetAllCustomersAsync();
    }
}
