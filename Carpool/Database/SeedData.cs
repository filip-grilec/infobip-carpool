using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Carpool.Data;

namespace Carpool.Database
{
    public static class SeedData
    {
        static SeedData()
        {
            // Generates Repeatable datasets
            Randomizer.Seed = new Random(10_000);
        }
   
        public static List<Car> GenerateCars(int count)
        {
            var testCars = new Faker<Car>()
                .RuleFor(c => c.Name, f => f.Vehicle.Model())
                .RuleFor(c => c.Color, (f, c) => f.Commerce.Color())
                .RuleFor(c => c.Seats, (f, c) => f.Random.Number(4, 5))
                .RuleFor(c => c.CarType, (f, c) => f.Vehicle.Type())
                .RuleFor(u => u.LicencePlate, GenerateLicencePlate);

            var generatedCars = testCars.Generate(count);
            

            int id = 1;
            foreach (var car in generatedCars)
            {
                car.CarId = id++;
            }
            
            
            return generatedCars;
        }

        public static List<Employee> GenerateEmployees(int count)
        {
            var croatianNames = new Bogus.DataSets.Name("hr");

            var testEmployees = new Faker<Employee>()
                .RuleFor(e => e.EmployeeName, (f, e) => croatianNames.FullName())
                .RuleFor(e => e.HasDriverLicence, f => f.Random.Bool());


            var generatedEmployees = testEmployees.Generate(count);

            int id = 1;
            foreach (var employee in generatedEmployees)
            {
                employee.EmployeeId = id++;
                
            }
            return generatedEmployees;
        }

        public static List<Location> GenerateLocations(int count)
        {
            var croatianLocations = new Bogus.DataSets.Address("hr");

            var locations = new Faker<Location>()
                .RuleFor(l => l.Name, f => croatianLocations.City()).Generate(count);

            int id = 1;
            foreach (var location in locations)
            {
                location.LocationId = id;
                id++;
            }

            return locations;
        }

        private static string GenerateLicencePlate(Faker faker)
        {
            var cities = new[] {"ZG", "RI", "ST", "PU", "DA", "OS", "ZD", "DU", "KA"};

            return
                $"{faker.PickRandom(cities)} {faker.Random.Int(1_000, 10_000)}-{faker.Random.String(2, 2, 'A', 'V')}";
        }
    }
}