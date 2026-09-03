using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Course
    {
        public required string IdCourse { get; set; }
        public required Teacher IdTeacher { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Credits { get; set; }
        public required DateTime Hours { get; set; }

    }
}
