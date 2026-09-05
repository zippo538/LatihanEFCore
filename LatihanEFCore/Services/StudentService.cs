using AutoMapper;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using Microsoft.EntityFrameworkCore;
using LatihanEFCore.Services.Interfaces;
using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.DTO.Responses.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<StudentDTO>> CreateStudent(CreateStudentDTO student)
        {
            var emailAlreadyUsed = _db.Students
                           .Any(item => item.Email == student.Email);

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

            _db.Students.Add(entity);
            _db.SaveChanges();

            var response = _mapper.Map<StudentDTO>(entity);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditambahkan.");
        }

        

        public async Task<ApiResponseDto<List<StudentDTO>>> GetAllStudents()
        {
            var students = _db.Students
                            .AsNoTracking()
                            .Include(item => item.Organization)
                            .Include(item => item.ActivityPoints)
                            .Include(item => item.Tuitions)
                            .Include(item => item.Courses)
                            .OrderBy(item => item.Name)
                            .ToList();

            var response = _mapper.Map<List<StudentDTO>>(students);

            return ApiResponseDto<List<StudentDTO>>.SuccessResult(
                response,
                "Data mahasiswa berhasil diambil.");
        }

        public async Task<ApiResponseDto<StudentDTO>> GetStudent(int id)
        {
            var student = _db.Students
                           .AsNoTracking()
                           .Include(item => item.Organization)
                           .Include(item => item.ActivityPoints)
                           .Include(item => item.Tuitions)
                           .Include(item => item.Courses)
                           .FirstOrDefault(item => item.IdStudent == id);

            if (student is null)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }
            

            var response = _mapper.Map<StudentDTO>(student);
            response.Courses = _mapper.Map<List<CourseDTO>>(student.Courses);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditemukan.");
        }

        public async Task<ApiResponseDto<StudentDTO>> UpdateStudent(int id, UpdateStudentDTO student)
        {
            var entity = _db.Students
                            .Include(item => item.ActivityPoints)
                            .Include(item => item.Tuitions)
                            .Include(item => item.Courses)
                            .FirstOrDefault(item => item.IdStudent == id);

            if (entity is null)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            var emailAlreadyUsed = _db.Students.Any(item =>
                item.IdStudent != id && item.Email == student.Email);

            if (emailAlreadyUsed)
            {
                return ApiResponseDto<StudentDTO>.ErrorResult(
                    "Data mahasiswa gagal diubah.",
                    new List<string>
                    {
                                    $"Email {student.Email} sudah digunakan mahasiswa lain."
                    });
            }

            _mapper.Map(student, entity);
            _db.SaveChanges();

            var response = _mapper.Map<StudentDTO>(entity);

            return ApiResponseDto<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil diubah.");
        }
        public async Task<ApiResponseDto<bool>> DeleteStudent(int id)
        {
            var student = _db.Students.Find(id);

            if (student is null)
            {
                return ApiResponseDto<bool>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            _db.Students.Remove(student);
            _db.SaveChanges();

            return ApiResponseDto<bool>.SuccessResult(
                true,
                "Data mahasiswa berhasil dihapus.")
                ;
        }
    }
}
