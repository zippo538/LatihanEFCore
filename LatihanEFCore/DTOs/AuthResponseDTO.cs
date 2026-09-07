using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}