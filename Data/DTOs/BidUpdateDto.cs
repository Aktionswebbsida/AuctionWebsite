using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.DTOs
{
    public class BidUpdateDto
    {
        public int BidID { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero.")]
        public decimal BidAmount { get; set; }

        public DateTime BidDate { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int AdID { get; set; }
    }
}
