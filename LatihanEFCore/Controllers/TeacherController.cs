using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;


namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public sealed class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TeacherController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var teachers = await _db.Teachers
                .AsNoTracking()
                .OrderBy(teacher => teacher.Name)
                .Select(teacher => new
                {
                    teacher.IdTeacher,
                    teacher.Name,
                    teacher.Email,
                    teacher.HireDate,
                    teacher.Address,
                    teacher.PhoneNumber,
                    teacher.Department
                })
                .ToListAsync(cancellationToken);

            return Ok(teachers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var teacher = await _db.Teachers
                .AsNoTracking()
                .Where(teacher => teacher.IdTeacher == id)
                .Select(teacher => new
                {
                    teacher.IdTeacher,
                    teacher.Name,
                    teacher.Email,
                    teacher.HireDate,
                    teacher.Address,
                    teacher.PhoneNumber,
                    teacher.Department
                })
                .FirstOrDefaultAsync(cancellationToken);

            return teacher is null
                ? NotFound(new { message = $"Teacher dengan ID {id} tidak ditemukan." })
                : Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] TeacherRequest request,
            CancellationToken cancellationToken)
        {
            if (request.HireDate == default)
            {
                ModelState.AddModelError(nameof(request.HireDate), "Tanggal bergabung wajib diisi.");
                return ValidationProblem(ModelState);
            }

            var email = request.Email.Trim();
            if (await _db.Teachers.AnyAsync(t => t.Email == email, cancellationToken))
                return Conflict(new { message = $"Email {email} sudah digunakan." });

            var teacher = new Teacher
            {
                IdTeacher = 0,
                Name = request.Name.Trim(),
                Email = email,
                HireDate = request.HireDate,
                Address = request.Address.Trim(),
                PhoneNumber = request.PhoneNumber,
                Department = request.Department.Trim()
            };

            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = teacher.IdTeacher }, teacher);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] TeacherRequest request,
            CancellationToken cancellationToken)
        {
            var teacher = await _db.Teachers
                .FirstOrDefaultAsync(teacher => teacher.IdTeacher == id, cancellationToken);

            if (teacher is null)
                return NotFound(new { message = $"Teacher dengan ID {id} tidak ditemukan." });

            var email = request.Email.Trim();
            if (await _db.Teachers.AnyAsync(
                    other => other.IdTeacher != id && other.Email == email,
                    cancellationToken))
            {
                return Conflict(new { message = $"Email {email} sudah digunakan teacher lain." });
            }

            teacher.Name = request.Name.Trim();
            teacher.Email = email;
            teacher.HireDate = request.HireDate;
            teacher.Address = request.Address.Trim();
            teacher.PhoneNumber = request.PhoneNumber;
            teacher.Department = request.Department.Trim();

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { message = "Data teacher berhasil diubah.", data = teacher });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var teacher = await _db.Teachers
                .FirstOrDefaultAsync(teacher => teacher.IdTeacher == id, cancellationToken);

            if (teacher is null)
                return NotFound(new { message = $"Teacher dengan ID {id} tidak ditemukan." });

            var usedByCourse = await _db.Courses.AnyAsync(
                course => EF.Property<int>(course, "TeacherId") == id,
                cancellationToken);

            var usedByOrganization = await _db.Organizations.AnyAsync(
                organization => EF.Property<int>(organization, "TeacherId") == id,
                cancellationToken);

            if (usedByCourse || usedByOrganization)
            {
                return Conflict(new
                {
                    message = "Teacher masih digunakan oleh course atau organization. Hapus relasinya terlebih dahulu."
                });
            }

            _db.Teachers.Remove(teacher);
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { message = $"Teacher dengan ID {id} berhasil dihapus." });
        }

    }
}
    public sealed class TeacherRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string Department { get; set; } = string.Empty;
    }
