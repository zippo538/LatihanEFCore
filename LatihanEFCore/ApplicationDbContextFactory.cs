using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false)
                .AddJsonFile(
                    "appsettings.Development.json",
                    optional: true,
                    reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' tidak ditemukan.");
            }

            var optionsBuilder =
                new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}