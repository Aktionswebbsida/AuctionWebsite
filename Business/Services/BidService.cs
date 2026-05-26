using Business.Interfaces;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class BidService : IBidInterface
    {
        public readonly IBidRepository _IBidRepository;
        public readonly IHubContext _HubContext;

        public BidService(IBidRepository IBidRepository, IHubContext hubContext)
        {
            _IBidRepository = IBidRepository;
            _HubContext = hubContext;
        }
        public async Task<BidCreateDto?> CreateBidAsync(BidCreateDto bid)
        {
            try
            {
                var newBid = new Bid
                {
                    BidAmount = bid.BidAmount,
                    BidDate = bid.BidDate,
                    AdID = bid.AdID,
                    UserId = bid.UserId,
                };

                var result = await _IBidRepository.CreateBidAsync(newBid);
                if (result == null)
                {
                    return null;
                }

                await _IBidRepository.SaveChangesAsync();

                await _HubContext.Clients.Group($"adId:{bid.AdID}").SendAsync("UpdateHighestBid", bid.AdID, bid.BidAmount, bid.UserId);

                return bid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the bid: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteBid(int id)
        {
            try
            {
                var bidToDelete = await _IBidRepository.GetBidAsync(id);
                if (bidToDelete != null)
                {
                    await _IBidRepository.DeleteBid(bidToDelete);
                    await _IBidRepository.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the bid: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BidDto>> GetAllBidAsync()
        {
            try
            {
                var allbids = await _IBidRepository.GetAllBidAsync();

                var biddto = allbids.Select(x => new BidDto
                {
                    BidID = x.BidID,
                    BidAmount = x.BidAmount,
                    BidDate = x.BidDate,
                    UserId = x.UserId,
                    AdID = x.AdID,


                });

                return biddto;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving bids: {ex.Message}");
                throw;
            }
        }

        public async Task<BidDto?> GetBidAsync(int id)
        {
            try
            {
                var bidId = await _IBidRepository.GetBidAsync(id);
                if (bidId != null)
                {
                    return new BidDto
                    {
                        BidID = bidId.BidID,
                        BidAmount = bidId.BidAmount,
                        BidDate = bidId.BidDate,
                        UserId = bidId.UserId,
                        AdID = bidId.AdID,
                    };
                }
                return null;



            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving bid: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BidDto>> GetBidsbyAdId(int AdId)
        {
            try
            {
                var bids = await _IBidRepository.GetBidsByAdIdAsync(AdId);

                return bids.Select(x => new BidDto
                {
                    BidID = x.BidID,
                    BidAmount = x.BidAmount,
                    BidDate = x.BidDate,
                    UserId=x.UserId, 
                    AdID = x.AdID,
                }).ToList();
            }catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ad bids: {ex.Message}");
                throw;
            }
        }

        public async Task<BidUpdateDto?> UpdateBid(int id, BidUpdateDto bid)
        {
            try
            {
                var bidToUpdate = await _IBidRepository.GetBidAsync(id);
                 if(bidToUpdate == null)  return null;


                bidToUpdate.BidAmount = bid.BidAmount;
                    bidToUpdate.BidDate = bid.BidDate;
                    bidToUpdate.UserId = bid.UserId;
                    bidToUpdate.AdID = bid.AdID;

                   var result = await _IBidRepository.UpdateBid(bidToUpdate);
                if(result == null) return null;
                    await _IBidRepository.SaveChangesAsync();
                
                await _HubContext.Clients.Group($"adId:{bid.AdID}").SendAsync("UpdateHighestBid", bid.AdID, bid.BidAmount, bid.UserId);
                return bid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the bid: {ex.Message}");
                throw;
            }

        }
    }
}
