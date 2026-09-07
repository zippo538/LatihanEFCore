using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatihanEFCore.Commons;
using LatihanEFCore.DTOs;
using LatihanEFCore.Repository;

namespace LatihanEFCore.Services.Interfaces
{
    public interface ITeacherService 
    {
        Task<ServiceResult<TeacherDTO>> GetTeacher(int id);
        Task<ServiceResult<List<TeacherDTO>>> GetAllTeacher();
        Task<ServiceResult<TeacherDTO>> CreateTeacher(CreateTeacherDto teacher);
        Task<ServiceResult<TeacherDTO>> UpdateTeacher(int id, UpdateTeacherDto teacher);
        Task<ServiceResult<bool>> DeleteTeacher(int id);
    }
}