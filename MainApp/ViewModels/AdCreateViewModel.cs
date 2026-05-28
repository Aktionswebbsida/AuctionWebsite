using Data.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class AdCreateViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200, ErrorMessage = "Place cannot be longer than 200 characters.")]
        public string Place {  get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be greater than zero.")]
        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SellerId { get; set; }



        public ICollection<ImagesCreateViewModel> Images { get; set; } = new List<ImagesCreateViewModel>();
    }
}
