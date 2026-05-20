using Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IAdInterface
    {
        Task<AdCreateDto> CreateAdAsync(AdCreateDto adDto);
        Task<IEnumerable<AdDto>> GetAllAdsAsync();

        Task<IEnumerable<AdDto>> GetAllSellerAdsAsync(int sellerId);

        Task<AdDto?> GetAdByIdAsync(int id);

        Task<UpdateAdDto> UpdateAdAsync(int id, UpdateAdDto updateAdDto);

        Task DeleteAdAsync(int id);


    }
}
