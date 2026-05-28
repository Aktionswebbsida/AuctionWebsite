using Business.Interfaces;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class AdService : IAdInterface
    { 
        private readonly IAdRepository _addRepository;
        public readonly IHubContext _HubContext;

        public AdService(IAdRepository addRepository , IHubContext hubContext)
        {
            _addRepository = addRepository;
            _HubContext = hubContext;
        }

        public async Task<WinnerDTO?> AnnoceWinner(int adId)
        {
            try
            {
                var winner = await _addRepository.AnnoceWinner(adId);
                 if(winner == null) return null;

                if (winner.Bids == null || !winner.Bids.Any() )
                {
                    return null;
                }
                

                var highestbid = winner.Bids.OrderByDescending(x => x.BidAmount).FirstOrDefault();
                if(highestbid == null) return null;
                await _addRepository.SaveChangesAsync();

                var mappp = new WinnerDTO
                {
                    AdID = winner.AdID,
                    BidAmount = highestbid.BidAmount,
                    BidDate = highestbid.BidDate,
                    UserId = highestbid.UserId,
                    EndDate = winner.EndDate,
                    UserName    = (highestbid.User != null) ? (!string.IsNullOrWhiteSpace(highestbid.User.FirstName) ? $"{highestbid.User.FirstName} {highestbid.User.LastName}" : highestbid.User.UserName) : "Unknown bidde"
                };


                await _HubContext.Clients.Group($"adId:{mappp.AdID}").SendAsync("YouWon", mappp);
                return mappp;

                

            }catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while annoncing the winner: {ex.Message}");
                throw;
            }
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
                    await _addRepository.DeleteAdAsync(id);
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

        public async Task<BuyNowDTO?> BuyNow(int adId, int userId)
        {
            try
            {
                var winner = await _addRepository.BuyNow(adId, userId);
                if (winner == null) return null;

               
                await _addRepository.SaveChangesAsync();

                var mapp = new BuyNowDTO
                {
                    AdID = adId,
                    IsClosed = true,
                    WinnerId = userId,
                    IsSold = true,
                    Name = winner.Seller?.FirstName ?? "Umknown seller",

                };


                await _HubContext.Clients.Group($"adId:{mapp.AdID}").SendAsync("BuyNow",mapp );
                return mapp;



            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while annoncing the winner: {ex.Message}");
                throw;
            }
        }
    }
}
