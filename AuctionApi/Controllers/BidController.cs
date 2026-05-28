using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        public IBidInterface _bidService ;

        public BidController(IBidInterface bidService)
        {
            _bidService = bidService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllBids()
        {
            try
            {
                var Allbids = await _bidService.GetAllBidAsync();
                return Ok(Allbids);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ad/{adId:int}")]
        public async Task<IActionResult> GetBidsByAdId(int adId)
        {
            try
            {
                var adbids = await _bidService.GetBidsbyAdId(adId);
                return Ok(adbids);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{bidId:int}")]
        public async Task<IActionResult> GetOneBid(int bidId)
        {
            try
            {
                var bidID = await _bidService.GetBidAsync(bidId);
                return Ok(bidID);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
