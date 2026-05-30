using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class ImagesUpdateViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;

        
        
        public IFormFile? Url { get; set; }

        public string? ExistingUrls { get; set; }
    }
}
