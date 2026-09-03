using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Student
    {
        public required int IdStudent { get; set; }
        public required ActivityPoints IdActivityPoints { get; set; }
        public required Tuition IdTuition { get; set; }
        public Organization? IdOrganization { get; set; }

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required DateTime EnrollmentDate { get; set; }
        public required string PhoneNumber { get; set; }
        public required decimal GPA { get; set; }
    

        public string? Address { get; set; }

    }
}
