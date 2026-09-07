using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using LatihanEFCore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LatihanEFCore.Repository
{
    public class TeacherRepository : Repository<Teacher, int> , ITeacherRepository
    {
        
        public  TeacherRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Teacher>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Teachers
                .AsNoTracking()
                .Include(teacher => teacher.Organization)
                .Include(teacher => teacher.Courses)
                .OrderBy(teacher => teacher.IdTeacher)
                .ToListAsync(cancellationToken);
        }

        public async Task<Teacher?> GetWithRelationsAsync(int id, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Teacher> query = Context.Teachers
                .Include(teacher => teacher.Organization)
                .Include(teacher => teacher.Courses);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                teacher => teacher.IdTeacher == id,
                cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludedStudentId = null, CancellationToken cancellationToken = default)
        {
            return await Context.Teachers.AnyAsync(
                teacher =>
                    teacher.Email == email &&
                    (!excludedStudentId.HasValue ||
                     teacher.IdTeacher != excludedStudentId.Value),
                cancellationToken);
        }
    }
}