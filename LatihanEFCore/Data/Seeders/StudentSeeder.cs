using Bogus;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public static class StudentSeeder
    {
        public static List<Student> GetStudents(Teacher defaultTeacher, Course defaultCourse, int count = 10)
        {
            var studentId = 1;

            // Satu Teacher hanya dimiliki oleh satu Organization.
            // Seluruh Student yang dibuat oleh seeder ini berada pada organisasi yang sama.
            var organization = new Organization
            {
                IdOrganization = 1,
                Name = "Organization A",
                Address = "Jl. Pendidikan No. 1",
                PhoneNumber = "081234567890",
                Email = "organization.a@example.com",
                Description = "Organisasi utama untuk data awal.",
                IdTeacher = defaultTeacher.IdTeacher,
                Teacher = defaultTeacher
            };

            // Pool ActivityPoints dibuat satu kali agar satu point dapat digunakan
            // oleh banyak Student (relasi many-to-many).
            var activityPointId = 1;
            var activityPointFaker = new Faker<ActivityPoints>("id_ID")
                .RuleFor(a => a.IdActivityPoints, _ => activityPointId++)
                .RuleFor(a => a.Title, f => f.Lorem.Sentence(3))
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .RuleFor(a => a.Date, f => f.Date.Recent(30))
                .RuleFor(a => a.Points, f => f.Random.Number(10, 100));

            var activityPoints = activityPointFaker.Generate(Math.Max(3, count / 2));

            var tuitionId = 1;

            var faker = new Faker<Student>("id_ID") // Menggunakan lokal Indonesia
                .RuleFor(s => s.IdStudent, f => studentId++)
                .RuleFor(s => s.Name, f => f.Name.FullName()) // Generasi Nama Acak
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.Name)) // Email acak berdasarkan nama
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Past(3)) // Tanggal acak 3 tahun lalu
                .RuleFor(s => s.GPA, f => Math.Round(f.Random.Decimal(2.5m, 4.0m), 2))
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber("08##########"))
                .RuleFor(s => s.IdOrganization, _ => organization.IdOrganization)
                .RuleFor(s => s.Organization, _ => organization)
                .RuleFor(s => s.ActivityPoints, f => activityPoints
                    .Where((_, index) => index == 0 || f.Random.Bool())
                    .ToList())
                .RuleFor(s => s.Tuitions, (f, student) => new List<Tuition>
                {
                    new Tuition
                    {
                        IdTuition = tuitionId++,
                        IdStudent = student.IdStudent,
                        Student = student,
                        IdCourse = defaultCourse.IdCourse,
                        Course = defaultCourse,
                        Date = f.Date.Recent(60),
                        Amount = 5_000_000m
                    }
                });

            var students = faker.Generate(count); // Membuat `count` jumlah data secara otomatis

            // Lengkapi navigation property pada kedua sisi relasi.
            organization.Students = students;

            foreach (var student in students)
            {
                foreach (var activityPoint in student.ActivityPoints)
                {
                    activityPoint.Students.Add(student);
                }
            }

            return students;
        }
}
}

