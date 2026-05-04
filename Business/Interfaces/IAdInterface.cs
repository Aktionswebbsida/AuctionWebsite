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

        Task<UpdateAdDto?> GetAdByIdAsync(int id);

        Task UpdateAdAsync(UpdateAdDto adDto);

        Task DeleteAdAsync(int id);


    }
}
