using Data.DTOs;

namespace MainApp.ViewModels
{
    public class AdCreateViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SellerId { get; set; }



        public ICollection<ImagesCreateViewModel> Images { get; set; } = new List<ImagesCreateViewModel>();
    }
}
