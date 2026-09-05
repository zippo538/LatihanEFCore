using Bogus;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public class TeacherSeeder
    {
        public static List<Teacher> GetTeachers(int count = 5)
        {
            var teacherId = 1;
            var publicationId = 1;

            var departments = new[] { "Teknik Informatika", "Sistem Informasi", "Teknik Elektro", "Data Science" };

            // 1. Buat generator khusus untuk PublicationTeacher
            var publicationFaker = new Faker<PublicationTeacher>("id_ID")
                .RuleFor(p => p.IdPublicationTeacher, f => publicationId++)
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(4, 3)) // Judul publikasi acak
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Date, f => f.Date.Past(5)); // Tanggal publikasi 5 tahun terakhir

            // 2. Buat generator untuk Teacher
            var teacherFaker = new Faker<Teacher>("id_ID")
                .RuleFor(t => t.IdTeacher, f => teacherId++)
                .RuleFor(t => t.Name, f => f.Name.FullName())
                .RuleFor(t => t.Email, (f, t) => f.Internet.Email(t.Name))
                .RuleFor(t => t.HireDate, f => f.Date.Past(10))
                .RuleFor(t => t.Department, f => f.PickRandom(departments))
                .RuleFor(t => t.Address, f => f.Address.FullAddress())
                .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber("08##########"))
                // Generate 1 sampai 3 item PublicationTeacher untuk tiap Teacher
                .RuleFor(t => t.PublicationTeachers, f => publicationFaker.Generate(f.Random.Number(1, 3)));

            var teachers = teacherFaker.Generate(count);

            // 3. Hubungkan relasi timbal balik (Navigation Property IdTeacher pada PublicationTeacher)
            foreach (var teacher in teachers)
            {
                foreach (var publication in teacher.PublicationTeachers)
                {
                    publication.IdTeacher = teacher;
                }
            }

            return teachers;
        }
    }
}
