using LatihanEFCore.Commons;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ServiceResult<StudentDTO>> GetStudent(int id);
        Task<ServiceResult<List<StudentDTO>>> GetAllStudents();
        Task<ServiceResult<StudentDTO>> CreateStudent(CreateStudentDto student);
        Task<ServiceResult<StudentDTO>> UpdateStudent(int id, UpdateStudentDTO student);
        Task<ServiceResult<bool>> DeleteStudent(int id);

    }


}
