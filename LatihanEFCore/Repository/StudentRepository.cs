using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using LatihanEFCore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LatihanEFCore.Repository
{
    public class StudentRepository : Repository<Student, int>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Student>>
            GetAllWithRelationsAsync(
                CancellationToken cancellationToken = default)
        {
            return await Context.Students
                .AsNoTracking()
                .AsSplitQuery()
                .Include(student => student.Organization)
                .Include(student => student.ActivityPoints)
                .Include(student => student.Tuitions)
                .ThenInclude(tuition => tuition.Course)
                .Include(student => student.Courses)
                .OrderBy(student => student.IdStudent)
                .ToListAsync(cancellationToken);
        }

        public async Task<Student?> GetWithRelationsAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Student> query = Context.Students
                .AsSplitQuery()
                .Include(student => student.Organization)
                .Include(student => student.ActivityPoints)
                .Include(student => student.Tuitions)
                .ThenInclude(tuition => tuition.Course)
                .Include(student => student.Courses);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                student => student.IdStudent == id,
                cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            int? excludedStudentId = null,
            CancellationToken cancellationToken = default)
        {
            return await Context.Students.AnyAsync(
                student =>
                    student.Email == email &&
                    (!excludedStudentId.HasValue ||
                     student.IdStudent != excludedStudentId.Value),
                cancellationToken);
        }
    }
}