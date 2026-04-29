using Business.Interfaces;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        public readonly IAdInterface _adInterface;

        public AdController(IAdInterface adInterface)
        {
            _adInterface = adInterface;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAds()
        {
            try
            {
                var ads = await _adInterface.GetAllAdsAsync();
                return Ok(ads);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

      
         [HttpPost]
         public async Task<IActionResult> CreateAd(AdCreateDto adDto)
        {
            try
            {
                var createdAd = await _adInterface.CreateAdAsync(adDto);
                return Ok(createdAd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateAd(int id , UpdateAdDto adDto)
        {
            try
            {
                adDto.AdID = id;
                await _adInterface.UpdateAdAsync(adDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteAd(AdDto adDto)
        {
            try
            {
                await _adInterface.DeleteAdAsync(adDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    } 
}
