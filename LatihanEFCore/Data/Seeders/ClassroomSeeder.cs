using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data.Seeders
{
    public class ClassroomSeeder
    {
        public static List<Classroom> GetClassrooms(int count = 5)
        {
            char classroomId = 'A';
            int classroomNumber = 001;

            var faker = new Bogus.Faker<Classroom>("id_ID")
                .RuleFor(c => c.IdClassroom, f => $"CR_{classroomId}{classroomNumber++}")
                .RuleFor(c => c.Name, f => $"Ruang {f.Random.AlphaNumeric(3).ToUpper()}")
                .RuleFor(c => c.Capacity, f => f.Random.Number(20, 50))
                .RuleFor(c => c.Location, f => f.Address.FullAddress());

            return faker.Generate(count);
        }
    }
}
