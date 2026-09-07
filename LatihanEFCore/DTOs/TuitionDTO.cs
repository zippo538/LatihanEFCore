using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTOs
{
    public class TuitionDTO
    {
        public int IdTuition { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public CourseDto Course { get; set; } = null!;
    }
}