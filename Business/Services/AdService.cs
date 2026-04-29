using Business.Interfaces;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class AdService : IAdInterface
    { 
        private readonly IAddRepository _addRepository;

        public AdService(IAddRepository addRepository)
        {
            _addRepository = addRepository;
        }

        public async Task<AdDto> CreateAdAsync(AdDto adDto)
        {
            try
            {
                var AdDTO = new Ad
                {

                    Title = adDto.Title,
                    Description = adDto.Description,
                    StartingPrice = adDto.StartingPrice,
                    StartDate = adDto.StartDate,
                    EndDate = adDto.EndDate,
                    SellerId = adDto.SellerId
                }
                 ;
                 await _addRepository.AddAdAsync(AdDTO);
                await _addRepository.SaveChangesAsync();
               return adDto;

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while creating the ad: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }

        public async Task DeleteAdAsync(AdDto adDto)
        {
            try
            {
                var adToDelete = await _addRepository.GetAdByIdAsync(adDto.AdID);
                if (adToDelete != null)
                {
                    await _addRepository.DeleteAdAsync(adToDelete);
                    await _addRepository.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Ad with ID {adDto.AdID} not found.");
                }

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while deleting the ad: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller

            }
        }

        public async Task<IEnumerable<AdDto>> GetAllAdsAsync()
        {
            try
            {
                var ads = await _addRepository.GetAllAdsAsync();
                var adDtos = new List<AdDto>();
                foreach (var ad in ads)
                {
                    adDtos.Add(new AdDto
                    {
                        AdID = ad.AdID,
                        Title = ad.Title,
                        Description = ad.Description,
                        StartingPrice = ad.StartingPrice,
                        StartDate = ad.StartDate,
                        EndDate = ad.EndDate,
                        SellerId = ad.SellerId
                    });
                }
                return adDtos;

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ads: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }

        public async Task UpdateAdAsync(AdDto adDto)
        {
            try
            {
                var adToUpdate = await _addRepository.GetAdByIdAsync(adDto.AdID);
                if (adToUpdate != null)
                {
                    adToUpdate.Title = adDto.Title;
                    adToUpdate.Description = adDto.Description;
                    adToUpdate.StartingPrice = adDto.StartingPrice;
                    adToUpdate.StartDate = adDto.StartDate;
                    adToUpdate.EndDate = adDto.EndDate;
                    adToUpdate.SellerId = adDto.SellerId;

                    await _addRepository.UpdateAdAsync(adToUpdate);
                    await _addRepository.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Ad with ID {adDto.AdID} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while updating the ad: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }
    }
}
