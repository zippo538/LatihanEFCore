using Bogus;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public static class StudentSeeder
    {
        public static List<Student> GetStudents(Teacher defaultTeacher, Course defaultCourse, int count = 10)
        {
            var studentId = 1;
            List<string> nameOrganization = new List<string> { 
                "Organization A", 
                "Organization B", 
                "Organization C",
                "Organization D", 
                "Organization E", 
                "Organization F",
                "Organization G", 
                "Organization H", 
                "Organization I", 
                "Organization J"
                };

            var faker = new Faker<Student>("id_ID") // Menggunakan lokal Indonesia
                .RuleFor(s => s.IdStudent, f => studentId++)
                .RuleFor(s => s.Name, f => f.Name.FullName()) // Generasi Nama Acak
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.Name)) // Email acak berdasarkan nama
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Past(3)) // Tanggal acak 3 tahun lalu
                .RuleFor(s => s.GPA, f => Math.Round(f.Random.Decimal(2.5m, 4.0m), 2))
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(s => s.IdActivityPoints, f => new ActivityPoints
                {
                    IdActivityPoints = f.IndexGlobal,
                    IdStudent = studentId,
                    Title = f.Lorem.Sentence(3),
                    Description = f.Lorem.Paragraph(),
                    Date = f.Date.Recent(30),
                    Points = f.Random.Number(10, 100)
                })
                .RuleFor(s => s.IdTuition, f => new Tuition
                {
                    IdTuition = f.IndexGlobal,
                    IdStudent = null!,
                    IdCourse = defaultCourse,
                    Date = f.Date.Recent(60),
                    Amount = 5000000m
                })
                .RuleFor(s => s.IdOrganization, f => new Organization
                {
                    IdOrganization = f.IndexGlobal,
                    Name = f.PickRandom(nameOrganization),
                    Address = f.Address.StreetAddress(),
                    PhoneNumber = f.Phone.PhoneNumber(),
                    Email = f.Internet.Email(),
                    Description = f.Lorem.Sentence(),
                    IdTeacher = defaultTeacher,
                    IdStudent = null!
                });

            var students = faker.Generate(count); // Membuat `count` jumlah data secara otomatis

            // Menghubungkan relasi navigasi
            foreach (var student in students)
            {
                student.IdTuition.IdStudent = student;
                if (student.IdOrganization != null)
                {
                    student.IdOrganization.IdStudent = student;
                }
            }

            return students;
        }
    }
}

