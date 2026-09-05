using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class ActivityPoints
    {
        public  int IdActivityPoints { get; set; }
        public required string Title { get; set; }
        public  string? Description { get; set; }
        public DateTime? Date { get; set; }
        public required int Points { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
