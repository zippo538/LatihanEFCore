using Bogus;
using LatihanEFCore.DTOs;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public static class StudentSeeder
    {
        public static List<Student> GetStudents(
    List<Teacher> teachers,
    List<Course> courses,
    int count = 10)
        {
            if (courses is null || courses.Count == 0)
            {
                throw new ArgumentException(
                    "Daftar course tidak boleh kosong.",
                    nameof(courses));
            }

            if (teachers is null || teachers.Count == 0)
            {
                throw new ArgumentException(
                    "Daftar teacher tidak boleh kosong.",
                    nameof(teachers));
            }

            var studentId = 1;
            if (teachers.Count < 10)
            {
                throw new ArgumentException(
                    "Jumlah teacher minimal harus 10 karena setiap organization membutuhkan teacher yang berbeda.",
                    nameof(teachers));
            }

            Organization CreateOrganization(int id, Teacher teacher) => new()
            {
                IdOrganization = id,
                Name = $"Organization {id}",
                Address = $"Jl. Pendidikan No. {id}",
                PhoneNumber = $"0812345678{id:00}",
                Email = $"organization{id}@example.com",
                Description = $"Organisasi nomor {id}.",
                IdTeacher = teacher.IdTeacher,
                Teacher = teacher
            };

            // Relasi Teacher-Organization one-to-one:
            // satu teacher hanya boleh digunakan oleh satu organization.
            var organizations = new List<Organization>
            {
                CreateOrganization(1, teachers[0]),
                CreateOrganization(2, teachers[1]),
                CreateOrganization(3, teachers[2]),
                CreateOrganization(4, teachers[3]),
                CreateOrganization(5, teachers[4]),
                CreateOrganization(6, teachers[5]),
                CreateOrganization(7, teachers[6]),
                CreateOrganization(8, teachers[7]),
                CreateOrganization(9, teachers[8]),
                CreateOrganization(10, teachers[9])
            };

            // ID tuition dibuat unik untuk setiap student.
            var tuitionId = 1;

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

            var faker = new Faker<Student>("id_ID") // Menggunakan lokal Indonesia
                .RuleFor(s => s.IdStudent, f => studentId++)
                .RuleFor(s => s.Name, f => f.Name.FullName()) // Generasi Nama Acak
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.Name)) // Email acak berdasarkan nama
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Past(3).Date) // Tanggal acak 3 tahun lalu
                .RuleFor(s => s.GPA, f => Math.Round(f.Random.Decimal(2.5m, 4.0m), 2))
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber("08##########"))
                .RuleFor(s => s.Organization, f => f.PickRandom(organizations))
                .RuleFor(s => s.IdOrganization, (_, student) => student.Organization.IdOrganization)
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
                        Course = f.PickRandom(courses),
                        IdCourse = string.Empty,
                        Date = f.Date.Recent(60),
                        Amount = 5_000_000m
                    }
                });

            var students = faker.Generate(count); // Membuat `count` jumlah data secara otomatis

            // Lengkapi navigation property pada sisi Organization.
            foreach (var organization in organizations)
            {
                organization.Students = students
                    .Where(student => student.Organization == organization)
                    .ToList();
            }

            // Lengkapi ID course berdasarkan course yang dipilih secara acak.
            foreach (var student in students)
            {
                foreach (var tuition in student.Tuitions)
                {
                    tuition.IdCourse = tuition.Course.IdCourse;

                    // Lengkapi relasi many-to-many Student-Course.
                    if (!student.Courses.Contains(tuition.Course))
                    {
                        student.Courses.Add(tuition.Course);
                    }

                    if (!tuition.Course.Students.Contains(student))
                    {
                        tuition.Course.Students.Add(student);
                    }
                }
            }

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

