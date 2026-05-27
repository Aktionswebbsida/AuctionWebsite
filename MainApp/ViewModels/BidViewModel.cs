namespace MainApp.ViewModels
{
    public class BidViewModel
    {
        public int BidID { get; set; }

        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public int UserId { get; set; }


        public string? UserName { get; set; }
        public int AdID { get; set; }
    }
}
