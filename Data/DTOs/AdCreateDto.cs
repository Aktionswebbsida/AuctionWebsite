using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class AdCreateDto
    {
       
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SellerId { get; set; }



        public ICollection<ImagesCreateDto> Images { get; set; } = new List<ImagesCreateDto>();
    }
}
