using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Images
    {
        public int ImageID { get; set; }

       public string Description { get; set; } = string.Empty;
       
        public string Url { get; set; } = string.Empty;

        public int AdId { get; set; }

        public Ad Ad { get; set; } = null!;

    }
}
