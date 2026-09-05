using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponseDto<StudentDTO>> GetStudent(int id);
        Task<ApiResponseDto<List<StudentDTO>>> GetAllStudents();
        Task<ApiResponseDto<StudentDTO>> CreateStudent(CreateStudentDTO student);
        Task<ApiResponseDto<StudentDTO>> UpdateStudent(int id, UpdateStudentDTO student);
        Task<ApiResponseDto<bool>> DeleteStudent(int id);

    }


}
