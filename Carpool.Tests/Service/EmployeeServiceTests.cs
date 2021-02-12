using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using Xunit;

namespace OkrApp.Tests
{
    public class EmployeeServiceTests : TestWithInMemoryDatabase
    {
        public EmployeeServiceTests()
        {
            ClearDatabaseForConcurrencyReasons();
        }

        [Theory]
        [InlineData("2021-02-11", "2021-02-20")]
        [InlineData("2021-01-20", "2021-01-29")]
        public async Task AreEmployeesAvailable_WhenEmployeesHaveNoTripsAtTheSameTime_ReturnsAvailable(
            string tripStartString, string tripEndString)
        {
            // Arrange 

            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.First().HasDriverLicence = true; // at least one needs to have driversLicence

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            var addEmployees = employees.Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var employeeIds = employees.Select(e => e.EmployeeId);
            var startDate = DateTime.Parse(tripStartString);
            var endDate = DateTime.Parse(tripEndString);
            var (success, errorMessage) = await employeeService.AreEmployeesAvailable(employeeIds,
                startDate, endDate);

            // Assert
            success.Should().BeTrue();
        }
        [Theory]
        [InlineData("2021-02-11", "2021-02-20")]
        [InlineData("2021-01-20", "2021-01-29")]
        public async Task GetAvailableEmployees_WhenEmployeesHaveNoTripsAtTheSameTime_ReturnsEmployees(
            string tripStartString, string tripEndString)
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.First().HasDriverLicence = true; // at least one needs to have driversLicence

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            var addEmployees = employees.Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var startDate = DateTime.Parse(tripStartString);
            var endDate = DateTime.Parse(tripEndString);
            var (success, employeesResults) = await employeeService.GetAvailableEmployees(startDate, endDate);

            // Assert
            success.Should().BeTrue();
            employeesResults.Count().Should().Be(employees.Count);
        }

        [Fact]
        public async Task AreEmployeesAvailable_WhenNoEmployeesHasDriverLicence_ReturnsFalse()
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.ForEach(e => e.HasDriverLicence = false);

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var employeeIds = employees.Select(e => e.EmployeeId);
            var startDate = DateTime.Parse("2021-02-11");
            var endDate = DateTime.Parse("2021-02-20");
            var (success, errorMessage) = await employeeService.AreEmployeesAvailable(employeeIds,
                startDate, endDate);

            // Assert
            success.Should().BeFalse();

            errorMessage.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("2021-02-4", "2021-02-20")]
        [InlineData("2021-02-1", "2021-02-5")]
        [InlineData("2021-02-1", "2021-02-20")]
        [InlineData("2021-01-20", "2021-02-20")]
        [InlineData("2021-02-5", "2021-02-20")]
        public async Task AreEmployeesAvailable_WhenOneEmployeeOnAnotherTrip_ReturnsSuccessFalse(string tripStartString,
            string tripEndString)
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.First().HasDriverLicence = true; // at least one needs to have driversLicence

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            var addEmployees = employees.Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var employeeIds = employees.Select(e => e.EmployeeId);
            var startDate = DateTime.Parse(tripStartString);
            var endDate = DateTime.Parse(tripEndString);
            var (success, errorMessage) = await employeeService.AreEmployeesAvailable(employeeIds,
                startDate, endDate);

            // Assert
            success.Should().BeFalse();
            errorMessage.Should().NotBeEmpty();
        }
        [Theory]
        [InlineData("2021-02-4", "2021-02-20")]
        [InlineData("2021-02-1", "2021-02-5")]
        [InlineData("2021-02-1", "2021-02-20")]
        [InlineData("2021-01-20", "2021-02-20")]
        [InlineData("2021-02-5", "2021-02-20")]
        public async Task GetAvailableEmployees_WhenOneEmployeesOnAnotherTrip_ReturnsSuccessFalse(string tripStartString,
            string tripEndString)
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.First().HasDriverLicence = true; // at least one needs to have driversLicence

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            var addEmployees = employees.Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var startDate = DateTime.Parse(tripStartString);
            var endDate = DateTime.Parse(tripEndString);
            var (success, employeeResults) = await employeeService.GetAvailableEmployees(startDate, endDate);

            // Assert
            success.Should().BeFalse();
            employeeResults.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAvailableEmployees_WhenNoEmployees_ReturnsSuccessFalse()
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(3);
            employees.ForEach(e => e.HasDriverLicence = true);
            

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10),
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };
            
            var addEmployees = employees.Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);
            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.TravelPlans.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();

            var employeeService = EmployeeServiceFactory();

            // Act
            var startDate = DateTime.Parse("2021-02-01");
            var endDate = DateTime.Parse("2021-02-11");
            var (success, employeeResults) = await employeeService.GetAvailableEmployees(startDate, endDate);
            
            // Assert
            success.Should().BeFalse();

            employeeResults.Should().BeEmpty();
        }



        private IEmployeeService EmployeeServiceFactory() => new EmployeeService(DbContext);
    }
}