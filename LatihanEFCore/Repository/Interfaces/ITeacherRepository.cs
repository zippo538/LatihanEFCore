using LatihanEFCore.DTOs;

namespace LatihanEFCore.Repository
{
    public interface ITeacherRepository : IRepository<Teacher, int>
    {
        Task<IReadOnlyList<Teacher>> GetAllWithRelationsAsync(
            CancellationToken cancellationToken = default);

        Task<Teacher?> GetWithRelationsAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(
            string email,
            int? excludedStudentId = null,
            CancellationToken cancellationToken = default);
    }
}