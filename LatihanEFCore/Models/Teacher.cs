using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Teacher
    {
        public required int IdTeacher { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required DateTime HireDate { get; set; }
        public string? Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Department { get; set; }
        public ICollection<PublicationTeacher> PublicationTeachers { get; set; } = new List<PublicationTeacher>();
    }
}
