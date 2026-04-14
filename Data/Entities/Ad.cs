using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Ad
    {
        public int AdID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SellerId { get; set; }

        public User Seller { get; set; } = null!;


      

        public ICollection<Images> Images { get; set; } = new List<Images>();

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
