using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTOs
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
