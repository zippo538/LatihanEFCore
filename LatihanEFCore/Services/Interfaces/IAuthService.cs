using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponseDto<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<ApiResponseDto<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ApiResponseDto<UserDTO>> GetCurrentUserAsync(string userId);


    }
}