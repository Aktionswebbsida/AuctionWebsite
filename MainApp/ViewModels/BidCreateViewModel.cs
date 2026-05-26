using System.ComponentModel.DataAnnotations;

namespace MainApp.ViewModels
{
    public class BidCreateViewModel
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero.")]
        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public int UserId { get; set; }



        public int AdID { get; set; }
    }
}
