using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;
    }
}
