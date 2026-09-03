using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models
{
    public class Organization
    {
        public required int IdOrganization { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required Student IdStudent { get; set; }
        public required Teacher IdTeacher { get; set; }
        public  string? Description { get; set; }

    }
}
