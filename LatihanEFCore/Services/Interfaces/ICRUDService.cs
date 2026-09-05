using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.DTO.Responses;

namespace LatihanEFCore.Services.Interfaces
{
    public interface ICRUDService
    {
        public Task<ApiResponseDto<TResponse>> Create<TRequest, TResponse>(TRequest request);
    }
}