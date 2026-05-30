using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface ImgConvertInterface
    {
        Task<string> ConvertImageAsync(IFormFile file);
    }
}
