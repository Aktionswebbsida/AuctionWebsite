using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class ImagesDto
    {
        public int ImageID { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public int AdID { get; set; }

        public AdDto Ad { get; set; } = null!;
    }
}
