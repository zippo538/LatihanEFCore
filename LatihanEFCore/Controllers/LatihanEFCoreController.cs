using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{

    [ApiController]
    [Route("api/students")]
    public sealed class LatihanEFCoreController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

    public LatihanEFCoreController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /api/students
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var students = await _db.Students
            .AsNoTracking()
            .OrderBy(student => student.Name)
            .Select(student => new
            {
                student.IdStudent,
                student.Name,
                student.Email,
                student.PhoneNumber,
                student.Address,
                student.EnrollmentDate,
                student.GPA
            })
            .ToListAsync(cancellationToken);

        return Ok(students);
    }

    // GET: /api/students/search?keyword=budi
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? keyword,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { message = "Keyword pencarian wajib diisi." });
        }

        keyword = keyword.Trim();
        var isPhoneNumber = int.TryParse(keyword, out var phoneNumber);

        var students = await _db.Students
            .AsNoTracking()
            .Where(student =>
                EF.Functions.Like(student.Name, $"%{keyword}%") ||
                EF.Functions.Like(student.Email, $"%{keyword}%") ||
                EF.Functions.Like(student.Address, $"%{keyword}%"))
            .OrderBy(student => student.Name)
            .Select(student => new
            {
                student.IdStudent,
                student.Name,
                student.Email,
                student.PhoneNumber,
                student.Address,
                student.EnrollmentDate,
                student.GPA,
                TuitionId = student.IdTuition != null
                    ? student.IdTuition.IdTuition
                    : (int?)null,
                ActivityPointsId = student.IdActivityPoints != null
                    ? student.IdActivityPoints.IdActivityPoints
                    : (int?)null,
                OrganizationId = student.IdOrganization != null
                    ? student.IdOrganization.IdOrganization
                    : (int?)null
            })
            .Take(50)
            .ToListAsync(cancellationToken);

        return Ok(new { keyword, total = students.Count, data = students });
    }

    // GET: /api/students/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var student = await _db.Students
            .AsNoTracking()
            .Where(student => student.IdStudent == id)
            .Select(student => new
            {
                student.IdStudent,
                student.Name,
                student.Email,
                student.PhoneNumber,
                student.Address,
                student.EnrollmentDate,
                student.GPA,
                Tuition = student.IdTuition != null ? new
                {
                    student.IdTuition.IdTuition,
                    student.IdTuition.Amount,
                    student.IdTuition.Date
                } : null,
                ActivityPoints = student.IdActivityPoints != null ? new
                {
                    student.IdActivityPoints.IdActivityPoints,
                    student.IdActivityPoints.Title,
                    student.IdActivityPoints.Points
                } : null,
                Organization = student.IdOrganization != null ? new
                {
                    student.IdOrganization.IdOrganization,
                    student.IdOrganization.Name,
                    student.IdOrganization.Email
                } : null,
                Course = student.IdTuition != null && student.IdTuition.IdCourse != null
                    ? new
                    {
                        student.IdTuition.IdCourse.IdCourse,
                        student.IdTuition.IdCourse.Title,
                        student.IdTuition.IdCourse.Credits
                    }
                    : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (student is null)
        {
            return NotFound(new
            {
                message = $"Data mahasiswa dengan ID {id} tidak ditemukan."
            });
        }

        return Ok(new
        {
            message = "Data mahasiswa berhasil ditemukan.",
            data = student
        });
    }

    // POST: /api/students
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] StudentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.EnrollmentDate == default)
        {
            ModelState.AddModelError(
                nameof(request.EnrollmentDate),
                "Tanggal pendaftaran wajib diisi.");
            return ValidationProblem(ModelState);
        }

        var email = request.Email.Trim();
        var emailAlreadyUsed = await _db.Students
            .AnyAsync(student => student.Email == email, cancellationToken);

        if (emailAlreadyUsed)
        {
            return Conflict(new { message = $"Email {email} sudah digunakan." });
        }

        var student = new Student
        {
            IdStudent = 0,
            Name = request.Name.Trim(),
            Email = email,
            EnrollmentDate = request.EnrollmentDate,
            PhoneNumber = request.PhoneNumber,
            GPA = request.GPA,
            Address = request.Address.Trim(),

            // Relasi dependent dapat dibuat melalui endpoint masing-masing.
            IdActivityPoints = null!,
            IdTuition = null!
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.IdStudent },
            new
            {
                message = "Data mahasiswa berhasil ditambahkan.",
                data = ToStudentResponse(student)
            });
    }

    // PUT: /api/students/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] StudentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.EnrollmentDate == default)
        {
            ModelState.AddModelError(
                nameof(request.EnrollmentDate),
                "Tanggal pendaftaran wajib diisi.");
            return ValidationProblem(ModelState);
        }

        var student = await _db.Students
            .FirstOrDefaultAsync(student => student.IdStudent == id, cancellationToken);

        if (student is null)
        {
            return NotFound(new
            {
                message = $"Data mahasiswa dengan ID {id} tidak ditemukan."
            });
        }

        var email = request.Email.Trim();
        var emailAlreadyUsed = await _db.Students
            .AnyAsync(other =>
                other.IdStudent != id && other.Email == email,
                cancellationToken);

        if (emailAlreadyUsed)
        {
            return Conflict(new
            {
                message = $"Email {email} sudah digunakan oleh mahasiswa lain."
            });
        }

        student.Name = request.Name.Trim();
        student.Email = email;
        student.EnrollmentDate = request.EnrollmentDate;
        student.PhoneNumber = request.PhoneNumber;
        student.GPA = request.GPA;
        student.Address = request.Address.Trim();

        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            message = "Data mahasiswa berhasil diubah.",
            data = ToStudentResponse(student)
        });
    }

    // DELETE: /api/students/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var student = await _db.Students
            .FirstOrDefaultAsync(student => student.IdStudent == id, cancellationToken);

        if (student is null)
        {
            return NotFound(new
            {
                message = $"Data mahasiswa dengan ID {id} tidak ditemukan."
            });
        }

        // Organization memakai DeleteBehavior.Restrict pada DbContext.
        var hasOrganization = await _db.Organizations
            .AnyAsync(organization =>
                EF.Property<int>(organization, "StudentId") == id,
                cancellationToken);

        if (hasOrganization)
        {
            return Conflict(new
            {
                message = "Mahasiswa masih terhubung dengan organisasi. Hapus relasi organisasi terlebih dahulu."
            });
        }

        _db.Students.Remove(student);
        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            message = $"Data mahasiswa dengan ID {id} berhasil dihapus."
        });
    }

    private static object ToStudentResponse(Student student) => new
    {
        student.IdStudent,
        student.Name,
        student.Email,
        student.EnrollmentDate,
        student.PhoneNumber,
        student.GPA,
        student.Address
    };
    }
}
public sealed class StudentRequest
{
    [Required(ErrorMessage = "Nama wajib diisi.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }

    [Required(ErrorMessage = "Nomor telepon wajib diisi.")]
    public string PhoneNumber { get; set; }

    [Range(typeof(decimal), "0", "4", ErrorMessage = "GPA harus berada pada rentang 0 sampai 4.")]
    public decimal GPA { get; set; }

    [Required(ErrorMessage = "Alamat wajib diisi.")]
    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;
}
