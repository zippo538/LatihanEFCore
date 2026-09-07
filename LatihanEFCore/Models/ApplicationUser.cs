using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LatihanEFCore.DTOs
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}