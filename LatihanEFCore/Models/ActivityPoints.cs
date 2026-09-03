using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class ActivityPoints
    {
        public required int IdActivityPoints { get; set; }
        public required int IdStudent { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? Date { get; set; }
        public required int Points { get; set; }
    }
}
