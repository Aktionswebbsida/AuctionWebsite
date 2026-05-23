using Data.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IBidInterface
    {
        public Task<BidCreateDto> CreateBidAsync(BidCreateDto bid);

        public Task<BidUpdateDto> UpdateBid(int id,BidUpdateDto bid);

        public Task DeleteBid(int id);

        public Task<IEnumerable<BidDto>> GetAllBidAsync();

        public Task<BidDto?> GetBidAsync(int id);
    }
}
