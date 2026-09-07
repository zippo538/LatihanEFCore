using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTOs
{
    public class Teacher
    {
        public int IdTeacher { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string? Address { get; set; }
        public required string PhoneNumber { get; set; }
        public string Department { get; set; } = null!;
        public Organization Organization { get; set; } = null!;
        public string idCourse { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public ICollection<PublicationTeacherDto> PublicationTeachers { get; set; } = new List<PublicationTeacherDto>();
    }
}
