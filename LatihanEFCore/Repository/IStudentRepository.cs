using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace LatihanEFCore.Repository
{
    public interface IStudentRepository : IRepository<Student, int>
    {
        Task<IReadOnlyList<Student>> GetAllWithRelationsAsync(
            CancellationToken cancellationToken = default);

        Task<Student?> GetWithRelationsAsync(
            int id,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(
            string email,
            int? excludedStudentId = null,
            CancellationToken cancellationToken = default);
    }
}