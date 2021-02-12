using System;
using System.Linq;
using Carpool.Data;
using Carpool.Database;
using Carpool.Validators;
using Microsoft.EntityFrameworkCore;

namespace Carpool
{
    public class CarpoolContext : DbContext
    {
        public CarpoolContext(DbContextOptions<CarpoolContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var generatedCars = SeedData.GenerateCars(10);
            
            modelBuilder.Entity<Car>().HasData(generatedCars);

            modelBuilder.Entity<Location>().HasData(SeedData.GenerateLocations(10));
            
            modelBuilder.Entity<Employee>().HasData(SeedData.GenerateEmployees(25));
            modelBuilder.Entity<TravelPlanEmployees>()
                .HasKey(te => new {te.EmployeeId, te.TravelPlanId});
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TravelPlan> TravelPlans { get; set; }
        public DbSet<TravelPlanEmployees> TravelPlanEmployees { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}