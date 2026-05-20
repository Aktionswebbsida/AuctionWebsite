using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IAdRepository
    {
        Task<Ad> AddAdAsync(Ad ad);
        Task UpdateAdAsync(Ad ad);

        Task DeleteAdAsync(Ad ad);

        Task<Ad?> GetAdByIdAsync(int adId);

        Task<IEnumerable<Ad>> GetAllAdsAsync();

        Task<IEnumerable<Ad>> GetAllSellerAds(int sellerId);

        Task SaveChangesAsync ();
    }
}
