using Business.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class ImgConveterService : ImgConvertInterface
    {
        private readonly IWebHostEnvironment _environment;

        public ImgConveterService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> ConvertImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("The provided file is null or empty.");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".avif", ".bmp", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                bool isExtensionValid = allowedExtensions.Contains(extension);
                bool isMimeTypeValid = file.ContentType.StartsWith("image/");

               
                if (!isExtensionValid && !isMimeTypeValid)
                {
                    throw new InvalidOperationException("Invalid file format. Only images are allowed.");
                }

                string rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadsFolder = Path.Combine(rootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return "https://localhost:7284/uploads/" + uniqueFileName;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
