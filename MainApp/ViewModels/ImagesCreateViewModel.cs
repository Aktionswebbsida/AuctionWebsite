using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class ImagesCreateViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;
        [Required (ErrorMessage ="Please select a image to upload")]
        
        public IFormFile? Url { get; set; }

    }
}
