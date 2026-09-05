using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanEFCore.DTO.Responses.DTOs
{
    public class OrganizeDTO
    {
        public int IdOrganization { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}