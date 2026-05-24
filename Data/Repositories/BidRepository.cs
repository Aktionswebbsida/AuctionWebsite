using Data.DbContext;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContexts _dbContext;

        public BidRepository(ApplicationDbContexts dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Bid?> CreateBidAsync(Bid bid)
        {
            var higestbid = _dbContext.Bids.Include(x => x.Ad).Include(x => x.User).Where(x => x.AdID == bid.AdID).OrderByDescending(x => x.BidAmount).FirstOrDefault();
            if(higestbid != null && bid.BidAmount <= higestbid.BidAmount)
            {
                return null;
            }
          await _dbContext.Bids.AddAsync(bid);
            return bid;
        }

        public async Task DeleteBid(Bid bid)
        {
            _dbContext.Bids.Remove(bid);
        }

        public async Task<IEnumerable<Bid>> GetAllBidAsync()
        {
          var allbids =  await _dbContext.Bids.Include(x => x.Ad).Include(x => x.User).ToListAsync();
            return allbids;
        }

        public async Task<Bid?> GetBidAsync(int id)
        {
         return await _dbContext.Bids.Include(x => x.Ad).Include(x =>x.User).FirstOrDefaultAsync( x => x.BidID == id );
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBid(Bid bid)
        {
         
            _dbContext.Bids.Update(bid);
           
        }
    }
}
