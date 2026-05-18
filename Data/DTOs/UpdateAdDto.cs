using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Data.DTOs
{
    public class UpdateAdDto
    {
        [JsonIgnore]
        public int AdID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Place {  get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [JsonIgnore]
        public int SellerId { get; set; }






        public ICollection<UpdateImagesDto> Images { get; set; } = new List<UpdateImagesDto>();
    }
}
