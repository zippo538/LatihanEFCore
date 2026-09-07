using AutoMapper;
using LatihanEFCore.Commons;
using LatihanEFCore.DTOs;
using LatihanEFCore.Repository;
using LatihanEFCore.Services.Interfaces;

namespace LatihanEFCore.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;
    private readonly IMapper _mapper;

    public TeacherService(ITeacherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ServiceResult<TeacherDTO>> GetTeacher(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher is null)
        {
            return ServiceResult<TeacherDTO>.ErrorResult(
                "Data Dosen tidak ditemukan.",
                new List<string> { $"Dosen dengan ID {id} tidak ditemukan." });
        }

        return ServiceResult<TeacherDTO>.SuccessResult(
            _mapper.Map<TeacherDTO>(teacher),
            "Data Dosen berhasil ditemukan.");
    }

    public async Task<ServiceResult<List<TeacherDTO>>> GetAllTeacher()
    {
        var teachers = await _repository.GetAllAsync();
        var response = _mapper.Map<List<TeacherDTO>>(teachers);

        return ServiceResult<List<TeacherDTO>>.SuccessResult(
            response,
            "Data Dosen berhasil diambil.");
    }

    public async Task<ServiceResult<TeacherDTO>> CreateTeacher(CreateTeacherDto teacher)
    {
        var emailAlreadyUsed = await _repository.EmailExistsAsync(teacher.Email);

        if (emailAlreadyUsed)
        {
            return ServiceResult<TeacherDTO>.ErrorResult(
                "Data Dosen gagal ditambahkan.",
                new List<string>
                {
                    $"Email {teacher.Email} sudah digunakan."
                });
        }

        var entity = _mapper.Map<Teacher>(teacher);
        entity.HireDate = DateTime.UtcNow;

        await _repository.AddAsync(entity);

        var response = _mapper.Map<TeacherDTO>(entity);

        return ServiceResult<TeacherDTO>.SuccessResult(
            response,
            "Data Dosen berhasil ditambahkan.");
    }

    public async Task<ServiceResult<TeacherDTO>> UpdateTeacher(int id, UpdateTeacherDto teacher)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return ServiceResult<TeacherDTO>.ErrorResult(
                "Data Dosen gagal diperbarui.",
                new List<string> { $"Dosen dengan ID {id} tidak ditemukan." });
        }

        if (!string.Equals(entity.Email, teacher.Email, StringComparison.OrdinalIgnoreCase)
            && await _repository.EmailExistsAsync(teacher.Email))
        {
            return ServiceResult<TeacherDTO>.ErrorResult(
                "Data Dosen gagal diperbarui.",
                new List<string> { $"Email {teacher.Email} sudah digunakan." });
        }

        _mapper.Map(teacher, entity);
        await _repository.UpdateAsync(entity);
        var response = _mapper.Map<TeacherDTO>(entity);

        return ServiceResult<TeacherDTO>.SuccessResult(
            response,
            "Data Dosen berhasil diperbarui.");
    }

    public async Task<ServiceResult<bool>> DeleteTeacher(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return ServiceResult<bool>.ErrorResult(
                "Data Dosen gagal dihapus.",
                new List<string> { $"Dosen dengan ID {id} tidak ditemukan." });
        }

        await _repository.DeleteAsync(id);

        return ServiceResult<bool>.SuccessResult(
            true,
            "Data Dosen berhasil dihapus.");
    }
}