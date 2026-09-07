using AutoMapper;
using LatihanEFCore.Commons;
using LatihanEFCore.DTOs;
using LatihanEFCore.Repository;
using LatihanEFCore.Services.Interfaces;

namespace LatihanEFCore.Services
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

        public async Task<ServiceResult<StudentDTO>> CreateStudent(CreateStudentDto student)
        {
            var emailAlreadyUsed = await _studentRepository.EmailExistsAsync(student.Email);

            if (emailAlreadyUsed)
            {
                return ServiceResult<StudentDTO>.ErrorResult(
                    "Data mahasiswa gagal ditambahkan.",
                    new List<string>
                    {
                                   $"Email {student.Email} sudah digunakan."
                    });
            }

            var entity = _mapper.Map<Student>(student);
            entity.EnrollmentDate = DateTime.UtcNow;
            entity.GPA = 0;

            await _studentRepository.AddAsync(entity);

            var response = _mapper.Map<StudentDTO>(entity);

            return ServiceResult<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditambahkan.");
        }
        public async Task<ServiceResult<List<StudentDTO>>>
                    GetAllStudents()
        {
            var students =
                await _studentRepository.GetAllWithRelationsAsync();

            var response = _mapper.Map<List<StudentDTO>>(students);

            return ServiceResult<List<StudentDTO>>.SuccessResult(
                response,
                "Data mahasiswa berhasil diambil.");
        }

        public async Task<ServiceResult<StudentDTO>> GetStudent(int id)
        {
            var student =
                await _studentRepository.GetWithRelationsAsync(id);

            if (student is null)
            {
                return ServiceResult<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            var response = _mapper.Map<StudentDTO>(student);

            return ServiceResult<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil ditemukan.");
        }

        public async Task<ServiceResult<StudentDTO>> UpdateStudent(
            int id,
            UpdateStudentDTO request)
        {
            var entity =
                await _studentRepository.GetWithRelationsAsync(
                    id,
                    asNoTracking: false);

            if (entity is null)
            {
                return ServiceResult<StudentDTO>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            var emailAlreadyUsed =
                await _studentRepository.EmailExistsAsync(
                    request.Email,
                    id);

            if (emailAlreadyUsed)
            {
                return ServiceResult<StudentDTO>.ErrorResult(
                    "Data mahasiswa gagal diubah.",
                    new List<string>
                    {
                        $"Email {request.Email} sudah digunakan mahasiswa lain."
                    });
            }

            _mapper.Map(request, entity);

            await _studentRepository.UpdateAsync(entity);

            var response = _mapper.Map<StudentDTO>(entity);

            return ServiceResult<StudentDTO>.SuccessResult(
                response,
                "Data mahasiswa berhasil diubah.");
        }

        public async Task<ServiceResult<bool>> DeleteStudent(int id)
        {
            var student =
                await _studentRepository.GetByIdAsync(id);

            if (student is null)
            {
                return ServiceResult<bool>.ErrorResult(
                    $"Data mahasiswa dengan ID {id} tidak ditemukan.");
            }

            await _studentRepository.DeleteAsync(id);

            return ServiceResult<bool>.SuccessResult(
                true,
                "Data mahasiswa berhasil dihapus.");
        }



    }
}
