using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public readonly ImgConvertInterface _imgConvert;

        public ImageController(ImgConvertInterface imgConvert)
        {
            _imgConvert = imgConvert;
        }


        [HttpPost("ImgUpload")]
        public async Task<IActionResult> UoloadImg(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            try
            {
                string path = await _imgConvert.ConvertImageAsync(file);
                return Ok(new {Url = path});
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          


        }
    }
}
