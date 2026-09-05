using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Tuition
    {
        public  int IdTuition { get; set; }
        public int IdStudent { get; set; }
        public  Student Student { get; set; } = null!;
        public string IdCourse { get; set; } = null!;
        public  Course Course { get; set; } = null!;
        public  DateTime Date { get; set; }
        public  decimal Amount { get; set; }
    }
}
