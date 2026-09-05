using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTO.Responses.DTOs
{
    public class ClassroomDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int Capacity { get; set; }
    }
}