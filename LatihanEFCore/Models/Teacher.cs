using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Teacher
    {
        public  int IdTeacher { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public  DateTime HireDate { get; set; }
        public string? Address { get; set; }
        public required string PhoneNumber { get; set; }
        public  string Department { get; set; } = null!;
        public Organization Organization { get; set; } = null!;
        public int idCourse { get; set; } 
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public ICollection<PublicationTeacher> PublicationTeachers { get; set; } = new List<PublicationTeacher>();
    }
}
