using Data.DTOs;

namespace MainApp.ViewModels
{
    public class AdUpdateViewModel
    {
        public int AdID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Place {  get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SellerId { get; set; }




        public ICollection<ImagesUpdateViewModel> Images { get; set; } = new List<ImagesUpdateViewModel>();
    }
}
