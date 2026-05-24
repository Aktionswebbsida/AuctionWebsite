using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IBidRepository
    {
        public Task<Bid?> CreateBidAsync (Bid bid);

        public Task UpdateBid (Bid bid);

        public Task DeleteBid (Bid bid);

        public Task<IEnumerable<Bid>> GetAllBidAsync ();

        public Task<Bid?> GetBidAsync (int id);

        public Task SaveChangesAsync();
    }
}
