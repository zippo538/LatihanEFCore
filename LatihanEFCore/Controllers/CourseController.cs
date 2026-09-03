using System.ComponentModel.DataAnnotations;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{
    [ApiController]
    [Route("api/courses")]
public sealed class CourseController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CourseController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var courses = await _db.Courses
            .AsNoTracking()
            .OrderBy(course => course.Title)
            .Select(course => new
            {
                course.IdCourse,
                course.Title,
                course.Description,
                course.Credits,
                course.Hours,
                Teacher = new
                {
                    course.IdTeacher,
                    course.Teacher.Name
                },
                Classroom = new
                {
                    course.ClassroomId,
                    course.Classroom.Name,
                    course.Classroom.Location
                }
            })
            .ToListAsync(cancellationToken);

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var course = await _db.Courses
            .AsNoTracking()
            .Where(course => course.IdCourse == id)
            .Select(course => new
            {
                course.IdCourse,
                course.Title,
                course.Description,
                course.Credits,
                course.Hours,
                Teacher = new
                {
                    course.IdTeacher,
                    course.Teacher.Name,
                    course.Teacher.Email
                },
                Classroom = new
                {
                    course.ClassroomId,
                    course.Classroom.Name,
                    course.Classroom.Location,
                    course.Classroom.Capacity
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        return course is null
            ? NotFound(new { message = $"Course dengan ID {id} tidak ditemukan." })
            : Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CourseCreateRequest request,
        CancellationToken cancellationToken)
    {
        var id = request.IdCourse.Trim();
        if (await _db.Courses.AnyAsync(course => course.IdCourse == id, cancellationToken))
            return Conflict(new { message = $"ID course {id} sudah digunakan." });

        var teacher = await _db.Teachers.FirstOrDefaultAsync(
            teacher => teacher.IdTeacher == request.TeacherId,
            cancellationToken);

        if (teacher is null)
            return BadRequest(new { message = $"Teacher dengan ID {request.TeacherId} tidak ditemukan." });

        var classroomId = request.ClassroomId.Trim();
        var classroom = await _db.Classrooms.FirstOrDefaultAsync(
            classroom => classroom.IdClassroom == classroomId,
            cancellationToken);

        if (classroom is null)
            return BadRequest(new { message = $"Classroom dengan ID {classroomId} tidak ditemukan." });

        var course = new Course
        {
            IdCourse = id,
            IdTeacher = request.TeacherId,
            Teacher = teacher,
            ClassroomId = classroomId,
            Classroom = classroom,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Credits = request.Credits,
            Hours = request.Hours
        };

        _db.Courses.Add(course);
        await _db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = course.IdCourse }, new
        {
            message = "Course berhasil ditambahkan.",
            data = new
            {
                course.IdCourse,
                course.Title,
                TeacherId = teacher.IdTeacher,
                ClassroomId = classroom.IdClassroom
            }
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] CourseUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var course = await _db.Courses
            .FirstOrDefaultAsync(course => course.IdCourse == id, cancellationToken);

        if (course is null)
            return NotFound(new { message = $"Course dengan ID {id} tidak ditemukan." });

        var teacher = await _db.Teachers.FirstOrDefaultAsync(
            teacher => teacher.IdTeacher == request.TeacherId,
            cancellationToken);

        if (teacher is null)
            return BadRequest(new { message = $"Teacher dengan ID {request.TeacherId} tidak ditemukan." });

        var classroomId = request.ClassroomId.Trim();
        var classroom = await _db.Classrooms.FirstOrDefaultAsync(
            classroom => classroom.IdClassroom == classroomId,
            cancellationToken);

        if (classroom is null)
            return BadRequest(new { message = $"Classroom dengan ID {classroomId} tidak ditemukan." });

        course.IdTeacher = request.TeacherId;
        course.Teacher = teacher;
        course.ClassroomId = classroomId;
        course.Classroom = classroom;
        course.Title = request.Title.Trim();
        course.Description = request.Description.Trim();
        course.Credits = request.Credits;
        course.Hours = request.Hours;

        await _db.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Course berhasil diubah." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var course = await _db.Courses
            .FirstOrDefaultAsync(course => course.IdCourse == id, cancellationToken);

        if (course is null)
            return NotFound(new { message = $"Course dengan ID {id} tidak ditemukan." });

        var usedByTuition = await _db.Tuitions.AnyAsync(
            tuition => EF.Property<string>(tuition, "CourseId") == id,
            cancellationToken);

        if (usedByTuition)
        {
            return Conflict(new
            {
                message = "Course masih digunakan oleh tuition. Hapus relasi tuition terlebih dahulu."
            });
        }

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync(cancellationToken);
        return Ok(new { message = $"Course dengan ID {id} berhasil dihapus." });
    }
}

public sealed class CourseCreateRequest : CourseUpdateRequest
{
    [Required, MaxLength(50)]
    public string IdCourse { get; set; } = string.Empty;
}

public class CourseUpdateRequest
{
    [Range(1, int.MaxValue)]
    public int TeacherId { get; set; }

    [Required, MaxLength(50)]
    public string ClassroomId { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 24)]
    public int Credits { get; set; }

    public DateTime Hours { get; set; }
}
}
