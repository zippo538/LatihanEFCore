using System.ComponentModel.DataAnnotations;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public sealed class ClassroomController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
    public ClassroomController(ApplicationDbContext db) => _db = db;

     [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var classrooms = await _db.Classrooms
            .AsNoTracking()
            .OrderBy(classroom => classroom.Name)
            .ToListAsync(cancellationToken);

        return Ok(classrooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var classroom = await _db.Classrooms
            .AsNoTracking()
            .FirstOrDefaultAsync(classroom => classroom.IdClassroom == id, cancellationToken);

        return classroom is null
            ? NotFound(new { message = $"Classroom dengan ID {id} tidak ditemukan." })
            : Ok(classroom);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ClassroomCreateRequest request,
        CancellationToken cancellationToken)
    {
        var id = request.IdClassroom.Trim();
        if (await _db.Classrooms.AnyAsync(c => c.IdClassroom == id, cancellationToken))
            return Conflict(new { message = $"ID classroom {id} sudah digunakan." });

        var classroom = new Classroom
        {
            IdClassroom = id,
            Name = request.Name.Trim(),
            Location = request.Location.Trim(),
            Capacity = request.Capacity
        };

        _db.Classrooms.Add(classroom);
        await _db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = classroom.IdClassroom }, classroom);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] ClassroomUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var classroom = await _db.Classrooms
            .FirstOrDefaultAsync(classroom => classroom.IdClassroom == id, cancellationToken);

        if (classroom is null)
            return NotFound(new { message = $"Classroom dengan ID {id} tidak ditemukan." });

        classroom.Name = request.Name.Trim();
        classroom.Location = request.Location.Trim();
        classroom.Capacity = request.Capacity;

        await _db.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Data classroom berhasil diubah.", data = classroom });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var classroom = await _db.Classrooms
            .FirstOrDefaultAsync(classroom => classroom.IdClassroom == id, cancellationToken);

        if (classroom is null)
            return NotFound(new { message = $"Classroom dengan ID {id} tidak ditemukan." });

        var usedByCourse = await _db.Courses.AnyAsync(
            course => EF.Property<string>(course, "ClassroomId") == id,
            cancellationToken);

        if (usedByCourse)
        {
            return Conflict(new
            {
                message = "Classroom masih digunakan oleh course. Hapus atau pindahkan course terlebih dahulu."
            });
        }

        _db.Classrooms.Remove(classroom);
        await _db.SaveChangesAsync(cancellationToken);
        return Ok(new { message = $"Classroom dengan ID {id} berhasil dihapus." });
    }
    }
}

public sealed class ClassroomCreateRequest
{
    [Required, MaxLength(50)]
    public string IdClassroom { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacity { get; set; }
}

public sealed class ClassroomUpdateRequest
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacity { get; set; }
}

