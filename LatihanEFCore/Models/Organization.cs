using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Organization
    {
        public  int IdOrganization { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public  string Address { get; set; } = null!;
        public  string PhoneNumber { get; set; } = null!;
        public  ICollection<Student> Students { get; set; } = new List<Student>();
        public  int IdTeacher { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public  string? Description { get; set; }

    }
}
