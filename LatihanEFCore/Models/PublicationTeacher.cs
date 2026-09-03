using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class PublicationTeacher
    {
        public required int IdPublicationTeacher { get; set; }
        public required Teacher IdTeacher { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public  DateTime? Date { get; set; }
    }
}
