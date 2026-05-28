using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class WinnerDTO
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }

        public int AdID { get; set; }

        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
