using Data.DbContext;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class AdRepository : IAdRepository
    {
        private readonly ApplicationDbContexts _dbContext;

            public AdRepository(ApplicationDbContexts dbContext)
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
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).Include(x => x.Bids).ThenInclude(b => b.User).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Ad?> GetAdByIdAsync(int id)
        {
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).Include(x => x.Bids).ThenInclude(b => b.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(x=>x.AdID == id);

        }


        public async Task UpdateAdAsync (Ad ad)
        {
            
             _dbContext.Ads.Update(ad);
       


        }

        public async Task<Ad?> DeleteAdAsync(int id)
        {
            var Ad = await GetAdByIdAsync(id);
            if (Ad != null)
            {
                Ad.IsDeleted = true;
            }

            return Ad;

        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ad>> GetAllSellerAds(int sellerId)
        {
            return await _dbContext.Ads.Include(x => x.Seller).Include(x => x.Images).Where(x => x.SellerId == sellerId).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Ad?> AnnoceWinner(int adId)
        {
            var ad = await GetAdByIdAsync(adId);
             if(ad != null)
            {
                if (DateTime.Now < ad.EndDate)
                    return ad;
                
                ad.IsClosed = true;
               
                var winner = ad.Bids.Where(x => x.AdID == adId).OrderByDescending(x => x.BidAmount).FirstOrDefault();
                 if(winner != null)
                {
                    ad.WinnerId = winner.UserId;
                }

                 return ad;

            }
           



               
            return null;
        }

        public async Task<Ad?> BuyNow(int adId, int userId)
        {
            var ad = await GetAdByIdAsync(adId);
            if (ad != null)
            {
               

                ad.IsClosed = true;
                ad.IsSold = true;
                ad.WinnerId = userId;

            }

            return ad;
            

        }
    }
}
