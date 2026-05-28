using Business.Interfaces;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class UserService : IUserInterface
    {
        public readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllCustomersAsync()
        {
            try
            {

                var customer = await userRepository.GetAllCustomersAsync();

                return customer.Select(Customer => new UserDTO
                {
                    Id = Customer.Id,

                    FirstName = Customer.FirstName,
                    LastName = Customer.LastName,
                    Email = Customer.Email
                });

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while fetching students: {ex.Message}");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public  async Task<IEnumerable<UserDTO>> GetAllSellersAsync()
        {
            try
            {

                var sellers = await userRepository.GetAllSellersAsync();

                return sellers.Select(seller => new UserDTO
                {
                    Id = seller.Id,

                    FirstName = seller.FirstName,
                    LastName = seller.LastName,
                    Email = seller.Email
                });

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while fetching students: {ex.Message}");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public Task<UserDTO?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
