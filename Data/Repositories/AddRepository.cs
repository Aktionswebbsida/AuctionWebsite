using Data.DbContext;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class AddRepository : IAddRepository
    {
        private readonly ApplicationDbContexts _dbContext;

            public AddRepository(ApplicationDbContexts dbContext)
            {
                _dbContext = dbContext;
        }
        public async Task<Ad> AddAdAsync(Ad ad)
        {
            await _dbContext.Ads.AddAsync(ad);
            return ad;
        }

        public async Task<IEnumerable<Ad>> GetAllAdsAsync()
        {
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).ToListAsync();
        }

        public async Task<Ad?> GetAdByIdAsync(int id)
        {
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).FirstOrDefaultAsync(x=>x.AdID == id);

        }


        public async Task UpdateAdAsync (Ad ad)
        {
            
             _dbContext.Ads.Update(ad);
              await _dbContext.SaveChangesAsync();


        }

        public async Task DeleteAdAsync(Ad ad)
        {
            _dbContext.Ads.Remove(ad);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ad>> GetAllSellerAds(int sellerId)
        {
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).Where(x => x.SellerId == sellerId).ToListAsync();
        }
    }
}
