using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Data.DTOs
{
    public class UpdateAdDto
    {
        [JsonIgnore]
        public int AdID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200, ErrorMessage = "Place cannot be longer than 200 characters.")]
        public string Place {  get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be greater than zero.")]
        public decimal StartingPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [JsonIgnore]
        public int SellerId { get; set; }






        public ICollection<UpdateImagesDto> Images { get; set; } = new List<UpdateImagesDto>();

        public bool IsDeleted { get; set; }
    }
}
