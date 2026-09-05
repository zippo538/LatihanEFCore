using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Classroom
    {
        public  string IdClassroom { get; set; } = null!;
        public required string Name { get; set; }
        public  string Location { get; set; } = null!;
        public  int Capacity { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
