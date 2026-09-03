using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Classroom
    {
        public required string IdClassroom { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required int Capacity { get; set; }
    }
}
