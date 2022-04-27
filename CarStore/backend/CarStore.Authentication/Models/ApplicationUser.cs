using Microsoft.AspNetCore.Identity;
using System;

namespace CarStore.Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
