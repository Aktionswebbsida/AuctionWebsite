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
        private readonly IAdRepository _addRepository;

        public AdService(IAdRepository addRepository)
        {
            _addRepository = addRepository;
        }

        public async Task<AdCreateDto> CreateAdAsync(AdCreateDto adDto)
        {
            try
            {
                var AdDTO = new Ad
                {

                    Title = adDto.Title,
                    Description = adDto.Description,
                    Place = adDto.Place,
                    StartingPrice = adDto.StartingPrice,
                    StartDate = adDto.StartDate,
                    EndDate = adDto.EndDate,
                    SellerId = adDto.SellerId,
                    
                    

                    Images = adDto .Images.Select(img => new Images
                    {
                        Url = img.Url,
                        Description = img.Description

                    }).ToList()
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

        public async Task DeleteAdAsync(int id)
        {
            try
            {
                var adToDelete = await _addRepository.GetAdByIdAsync(id);
                if (adToDelete != null)
                {
                    await _addRepository.DeleteAdAsync(adToDelete);
                    await _addRepository.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Ad with ID {id} not found.");
                }

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while deleting the ad: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }
        }

        public async Task<AdDto?> GetAdByIdAsync(int id)
        {
            try
            {

                var ad = await _addRepository.GetAdByIdAsync(id);

                if (ad == null) return null;

                return new AdDto
                {
                    AdID = ad.AdID,
                    Title = ad.Title,
                    Description = ad.Description,
                    Place = ad.Place,
                    StartingPrice = ad.StartingPrice,
                    StartDate = ad.StartDate,
                    EndDate = ad.EndDate,
                    SellerId = ad.SellerId,
                    SellerName = ad.Seller?.FirstName ?? "Unkwon seller",

                    Images = ad.Images.Select(img => new ImagesDto
                    {
                        Url = img.Url,
                        Description = img.Description,
                    }).ToList(),

                };
            }catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ad: {ex.Message}");
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
                        Place = ad.Place,
                        StartingPrice = ad.StartingPrice,
                        StartDate = ad.StartDate,
                        EndDate = ad.EndDate,
                        SellerId = ad.SellerId,
                        SellerName = ad.Seller?.FirstName ?? "Umknown seller", // Assuming User entity has a Name property
                        Images = ad.Images.Select(img => new ImagesDto
                        {
                            Url = img.Url,
                            Description = img.Description
                        }).ToList()

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

        public  async Task<IEnumerable<AdDto>> GetAllSellerAdsAsync(int sellerId)
        {
            try
            {
                var ads = await _addRepository.GetAllSellerAds(sellerId);
                var adDtos = new List<AdDto>();
                foreach (var ad in ads)
                {
                    adDtos.Add(new AdDto
                    {
                        AdID = ad.AdID,
                        Title = ad.Title,
                        Description = ad.Description,
                        Place = ad.Place,
                        StartingPrice = ad.StartingPrice,
                        StartDate = ad.StartDate,
                        EndDate = ad.EndDate,
                        SellerId = ad.SellerId,
                        SellerName = ad.Seller?.FirstName ?? "Umknown seller", // Assuming User entity has a Name property
                        Images = ad.Images.Select(img => new ImagesDto
                        {
                            Url = img.Url,
                            Description = img.Description
                        }).ToList()

                    });
                }
                return adDtos;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving seller ads: {ex.Message}");
                throw;
            }
        }

        public async Task<UpdateAdDto> UpdateAdAsync(int id, UpdateAdDto adDto)
        {
            try
            {
                var adToUpdate = await _addRepository.GetAdByIdAsync(id);
                if (adToUpdate != null)
                {
                    adToUpdate.Title = adDto.Title;
                    adToUpdate.Description = adDto.Description;
                    adToUpdate.Place = adDto.Place;
                    adToUpdate.StartingPrice = adDto.StartingPrice;
                    adToUpdate.StartDate = adDto.StartDate;
                    adToUpdate.EndDate = adDto.EndDate;

                    adToUpdate.Images.Clear();
                   adToUpdate.Images = adDto.Images.Select(img => new Images
                    {
                        Url = img.Url,
                        Description = img.Description
                    }).ToList();

                    await _addRepository.UpdateAdAsync(adToUpdate);
                    await _addRepository.SaveChangesAsync();

                    
                }
                else
                {
                    Console.WriteLine($"Ad with ID {adDto.AdID} not found.");
                }
                return adDto;
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
