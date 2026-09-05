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
            int classroomNumber = 1;

            var faker = new Bogus.Faker<Classroom>("id_ID")
                .RuleFor(c => c.IdClassroom, _ => $"CR_{classroomNumber++:D3}")
                .RuleFor(c => c.Name, f => $"Ruang {f.Random.AlphaNumeric(3).ToUpper()}")
                .RuleFor(c => c.Capacity, f => f.Random.Number(20, 50))
                .RuleFor(c => c.Location, f =>
                {
                    var location = f.Address.FullAddress();
                    return location[..Math.Min(200, location.Length)];
                });

            return faker.Generate(count);
        }
    }
}
