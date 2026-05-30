using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.DTOs
{
    public class UpdateImagesDto
    {
        [Required]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        
        public string Url { get; set; } = string.Empty;


        
    }
}
