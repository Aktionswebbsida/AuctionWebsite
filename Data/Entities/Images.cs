using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class Images
    {
        [Key] 
        public int ImageID { get; set; }

       public string Description { get; set; } = string.Empty;
       
        public string Url { get; set; } = string.Empty;

        public int AdID { get; set; }

        public Ad Ad { get; set; } = null!;

    }
}
