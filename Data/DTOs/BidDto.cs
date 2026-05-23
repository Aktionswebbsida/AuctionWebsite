using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class BidDto
    {
        public int BidID { get; set; }

        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public int UserId { get; set; }

     

        public int AdID { get; set; }

       
    }
}
