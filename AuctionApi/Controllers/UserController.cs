using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserInterface UserService;

        public UserController(IUserInterface userService)
        {
            UserService = userService;
        }

        [HttpGet("Sellers")]

        public async Task<IActionResult> GetAllSellers()
        {
            try
            {
                var seller = await UserService.GetAllSellersAsync();
                return Ok(seller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("Customers")]

        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var custoemr = await UserService.GetAllCustomersAsync();
                return Ok(custoemr);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
    }
}
