using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Bid
    {
        public int BidID { get; set; }

        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int AdID { get; set; }

        public Ad Ad { get; set; } = null!;
    }
}
