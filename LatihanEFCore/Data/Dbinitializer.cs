

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data
{
    public class Dbinitializer : IDbinitializer
    {
        private readonly ApplicationDbContext _context;

        public Dbinitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Initialized()
        {
            // Pastikan database dibuat
            await _context.Database.EnsureCreatedAsync();

            // Cek apakah data sudah ada, jika belum maka lakukan seeding
            if (!_context.Teachers.Any())
            {
                var teachers = Seeders.TeacherSeeder.GetTeachers(5);
                await _context.Teachers.AddRangeAsync(teachers);
                await _context.SaveChangesAsync();
            }

            if (!_context.Classrooms.Any())
            {
                var classrooms = Seeders.ClassroomSeeder.GetClassrooms(5);
                await _context.Classrooms.AddRangeAsync(classrooms);
                await _context.SaveChangesAsync();
            }
            if (!_context.Courses.Any())
            {
                var courses = Seeders.CourseSeeder.GetCourses(teachers : _context.Teachers.ToList(), count: 10, classroom: _context.Classrooms.ToList());
                await _context.Courses.AddRangeAsync(courses);
                await _context.SaveChangesAsync();
            }
            if (!_context.Students.Any())
            {
                // Lakukan seeding data di sini
                var students = Seeders.StudentSeeder.GetStudents(defaultTeacher: _context.Teachers.FirstOrDefault()!, defaultCourse: _context.Courses.FirstOrDefault()!, count: 10);
                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();
            }


        }


    }
}
