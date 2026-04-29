using Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IAdInterface
    {
        Task<AdDto> CreateAdAsync(AdDto adDto);
        Task<IEnumerable<AdDto>> GetAllAdsAsync();

        Task UpdateAdAsync(AdDto adDto);

        Task DeleteAdAsync(AdDto adDto);


    }
}
