using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Tuition
    {
        public required int IdTuition { get; set; }
        public required Student IdStudent { get; set; }
        public required Course IdCourse { get; set; }
        public required DateTime Date { get; set; }
        public required decimal Amount { get; set; }
    }
}
