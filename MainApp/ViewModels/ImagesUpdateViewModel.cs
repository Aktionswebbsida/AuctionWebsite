using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class ImagesUpdateViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Url { get; set; } = string.Empty;
    }
}
