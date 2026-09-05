using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public static class CourseSeeder
    {
        public static List<Course> GetCourses(List<Teacher> teachers, List<Classroom> classrooms, int count = 10)
        {
            if (teachers.Count == 0)
                throw new ArgumentException("Daftar teacher tidak boleh kosong.", nameof(teachers));

            if (classrooms.Count == 0)
                throw new ArgumentException("Daftar classroom tidak boleh kosong.", nameof(classrooms));

            var courseCodeIndex = 101;

            // Daftar pilihan nama mata kuliah
            var courseTitles = new[]
            {
                "Pemrograman C# & .NET Core",
                "Algoritma dan Struktur Data",
                "Basis Data Lanjut",
                "Arsitektur Microservices",
                "Machine Learning Dasar",
                "Pengembangan Web API",
                "Jaringan Komputer",
                "Pemrograman Berorientasi Objek"
            };

            var faker = new Faker<Course>("id_ID")
                // Format Kode Matkul acak (misal: CS101, CS102)
                .RuleFor(c => c.IdCourse, f => $"CS{courseCodeIndex++}")
                // Memilih salah satu Dosen secara acak dari list yang tersedia
                .RuleFor(c => c.Title, f => f.PickRandom(courseTitles))
                .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
                .RuleFor(c => c.Credits, f => f.Random.Number(2, 4)) // SKS berkisar 2-4
                .RuleFor(c => c.Teacher, f => f.PickRandom(teachers))
                .RuleFor(c => c.IdTeacher, (_, c) => c.Teacher.IdTeacher)
                .RuleFor(c => c.Classroom, f => f.PickRandom(classrooms))
                .RuleFor(c => c.ClassroomId, (_, c) => c.Classroom.IdClassroom)
                .RuleFor(c => c.Hours, f => DateTime.Today.AddHours(f.Random.Number(7, 16))); // Jam perkuliahan

            return faker.Generate(count);
        }


    }
}
