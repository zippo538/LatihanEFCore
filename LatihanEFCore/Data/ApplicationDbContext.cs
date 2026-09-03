using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Classroom> Classrooms { get; set; } = null!;
        public DbSet<Tuition> Tuitions { get; set; } = null!;
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<ActivityPoints> ActivityPoints { get; set; } = null!;
        public DbSet<PublicationTeacher> PublicationTeachers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureStudent(modelBuilder);
            ConfigureTeacher(modelBuilder);
            ConfigureCourse(modelBuilder);
            ConfigureClassroom(modelBuilder);
            ConfigureTuition(modelBuilder);
            ConfigureOrganization(modelBuilder);
            ConfigureActivityPoints(modelBuilder);
            ConfigurePublicationTeacher(modelBuilder);
        }

        private static void ConfigureStudent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(e => e.IdStudent);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.GPA).HasPrecision(3, 2);
            });
        }

        private static void ConfigureTeacher(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>(entity =>
    {
        entity.ToTable("Teachers");
        entity.HasKey(e => e.IdTeacher);

        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
        entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
        entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        entity.Property(e => e.Department).IsRequired().HasMaxLength(100);

        // Menghubungkan secara eksplisit 2 arah:
        entity.HasMany(e => e.PublicationTeachers)
              .WithOne(p => p.IdTeacher) // <-- Tunjuk ke IdTeacher di PublicationTeacher
              .HasForeignKey("TeacherId")
              .OnDelete(DeleteBehavior.Cascade);
    });
        }

        private static void ConfigureCourse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(e => e.IdCourse);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Credits).IsRequired();
                entity.Property(e => e.IdTeacher).IsRequired();

                entity.Property(e => e.ClassroomId)
                    .IsRequired()
                    .HasMaxLength(50);

                // Relasi Course dengan Teacher
                entity.HasOne(e => e.Teacher)
                    .WithMany()
                    .HasForeignKey(e => e.IdTeacher)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relasi Course dengan Classroom
                entity.HasOne(e => e.Classroom)
                    .WithMany()
                    .HasForeignKey(e => e.ClassroomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureClassroom(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classrooms");
                entity.HasKey(e => e.IdClassroom);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            });
        }

        private static void ConfigureTuition(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tuition>(entity =>
            {
                entity.ToTable("Tuitions");
                entity.HasKey(e => e.IdTuition);

                entity.Property(e => e.Amount).HasPrecision(18, 2);

                entity.HasOne(e => e.IdStudent)
                    .WithOne(e => e.IdTuition)
                    .HasForeignKey<Tuition>("StudentId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.IdCourse)
                    .WithMany()
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureOrganization(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organizations");
                entity.HasKey(e => e.IdOrganization);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);

                entity.HasOne(e => e.IdStudent)
                    .WithOne(e => e.IdOrganization)
                    .HasForeignKey<Organization>("StudentId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.IdTeacher)
                    .WithMany()
                    .HasForeignKey("TeacherId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureActivityPoints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityPoints>(entity =>
            {
                entity.ToTable("ActivityPoints");
                entity.HasKey(e => e.IdActivityPoints);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);

                entity.HasOne<Student>()
                    .WithOne(e => e.IdActivityPoints)
                    .HasForeignKey<ActivityPoints>(e => e.IdStudent)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigurePublicationTeacher(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicationTeacher>(entity =>
            {
                entity.ToTable("PublicationTeachers");
                entity.HasKey(e => e.IdPublicationTeacher);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            });
        }
    }
}
