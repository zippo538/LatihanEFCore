using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.Commons;
using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDTO);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDTO);
        Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId);


    }
}