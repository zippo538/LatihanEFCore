using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTO.Responses.DTOs
{
    public class TeacherDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public string Address { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string Department { get; set; } = string.Empty;
    }
}