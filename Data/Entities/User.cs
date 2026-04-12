using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{

    
        public class User : IdentityUser<int>
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public string Street { get; set; } = string.Empty;

          public ICollection<Ad> Ads { get; set; } = new List<Ad>();



    

        }
}

