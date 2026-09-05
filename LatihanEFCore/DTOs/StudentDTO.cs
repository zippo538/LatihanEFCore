using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTO.Responses.DTOs
{
    public class StudentDTO
    {
        public int IdStudent { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public decimal GPA { get; set; }

        public int  IdOrganization { get; set; } 

        public List<ActivityPointDTO> ActivityPoints { get; set; }
            = new();

        public List<CourseDTO> Tuitions { get; set; }
            = new();
        public List<CourseDTO> Courses { get; set; }
            = new();
    }
}