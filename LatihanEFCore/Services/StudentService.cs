using AutoMapper;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using Microsoft.EntityFrameworkCore;
using LatihanEFCore.Services.Interfaces;
using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;
using LatihanEFCore.Repository;

namespace LatihanEFCore.DTO.Responses.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<StudentDTO>> CreateStudent(CreateStudentDTO student)
        {
            var emailAlreadyUsed = await _studentRepository.EmailExistsAsync(student.Email);

            if (emailAlreadyUsed)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    "Data mahasiswa gagal ditambahkan.",
                    new List<string>
                    {
                                   $"Email {student.Email} sudah digunakan."
                    });
            }

            var entity = _mapper.Map<home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models.Student>(student);
            entity.EnrollmentDate = DateTime.UtcNow;
            entity.GPA = 0;

            await _studentRepository.AddAsync(entity);

            var response = _mapper.Map<StudentDTO>(entity);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditambahkan.");
        }
        public async Task<ApiResponseDto<List<StudentDTO>>>
                    GetAllStudents()
        {
            var students =
                await _studentRepository.GetAllWithRelationsAsync();

            var response = _mapper.Map<List<StudentDTO>>(students);

            return ApiResponseDto<List<StudentDTO>>.SuccessResult(
                response,
                "Data mahasiswa berhasil diambil.");
        }

        public async Task<ApiResponseDto<StudentDTO>> GetStudent(int id)
        {
            var student =
                await _studentRepository.GetWithRelationsAsync(id);

            if (student is null)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            var response = _mapper.Map<StudentDTO>(student);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditemukan.");
        }

        public async Task<ApiResponseDto<StudentDTO>> UpdateStudent(
            int id,
            UpdateStudentDTO request)
        {
            var entity =
                await _studentRepository.GetWithRelationsAsync(
                    id,
                    asNoTracking: false);

            if (entity is null)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            var emailAlreadyUsed =
                await _studentRepository.EmailExistsAsync(
                    request.Email,
                    id);

            if (emailAlreadyUsed)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    "Data mahasiswa gagal diubah.",
                    new List<string>
                    {
                        $"Email {request.Email} sudah digunakan mahasiswa lain."
                    });
            }

            _mapper.Map(request, entity);

            await _studentRepository.UpdateAsync(entity);

            var response = _mapper.Map<StudentDTO>(entity);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil diubah.");
        }

        public async Task<ApiResponseDto<bool>> DeleteStudent(int id)
        {
            var student =
                await _studentRepository.GetByIdAsync(id);

            if (student is null)
            {
                return ApiResponseDto<bool>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            await _studentRepository.DeleteAsync(id);

            return ApiResponseDto<bool>.SuccessResult(
                true,
                "Data mahasiswa berhasil dihapus.");
        }



    }
}
