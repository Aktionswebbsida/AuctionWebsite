using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class BuyNowDTO
    {
        public int AdID { get; set; }
        public int UserId { get; set; }
        public bool IsClosed { get; set; }

        public string? Name { get; set; }
        public bool IsSold { get; set; }
        public int WinnerId { get; set; }

    }
}
