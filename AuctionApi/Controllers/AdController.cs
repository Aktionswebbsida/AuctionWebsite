using Business.Interfaces;
using Data.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        [HttpGet("seller/{sellerid:int}")]
        public async Task<IActionResult> GetAllSellerAds(int sellerid)
        {
            try
            {
                var ads = await _adInterface.GetAllSellerAdsAsync(sellerid);
                return Ok(ads);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetAdbyId(int id)
        {
            try
            {
                var AdbyId = await _adInterface.GetAdByIdAsync(id);
                return Ok(AdbyId);

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
                
                await _adInterface.UpdateAdAsync(id,adDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAd(int id)
        {
            try
            {
                await _adInterface.DeleteAdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    } 
}
