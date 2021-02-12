using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Carpool;
using Carpool.Contracts.Request;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using Carpool.Service.Interfaces;
using Carpool.Time;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using Xunit;

namespace OkrApp.Tests
{
    public class TravelPlanServiceTests : TestWithInMemoryDatabase
    {
        private Fixture _fixture = new Fixture();

        public TravelPlanServiceTests()
        {
            ClearDatabaseForConcurrencyReasons();
        }

        [Fact]
        public async Task CreateTravelPlan_WhenAllChecksPass_CreatesTravelPlan()
        {
            // Arrange 
            var availableCars = SeedData.GenerateCars(1);
            var availableCar = availableCars.First();

            var location = SeedData.GenerateLocations(1).First();
            DbContext.Add(location);

            DbContext.Add(availableCar);
            await DbContext.SaveChangesAsync();

            var employees = SeedData.GenerateEmployees(3);
            await DbContext.AddRangeAsync(employees);
            var carServiceStub = CarServiceIsCarAvailableReturns(true);

            var employeeServiceStub = EmployeeServiceReturnsEmployeesAvailable(true);

            var travelPlanService = TravelPlanServiceFactory(carServiceStub.Object, employeeServiceStub.Object, new SystemTimeProvider());

            var travelPlanOptions = _fixture.Create<TravelPlanOptions>();
            travelPlanOptions.CarId = availableCar.CarId;
            travelPlanOptions.StartDateUtc = DateTime.UtcNow;
            travelPlanOptions.EndDateUtc = DateTime.UtcNow.AddDays(3);
            travelPlanOptions.StartLocationId = location.LocationId;
            travelPlanOptions.EndLocationId = location.LocationId;
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();
            travelPlanOptions.EmployeeIds = employeeIds;

            // Act
            var (success, message,travelPlan) = await travelPlanService.CreateTravelPlan(travelPlanOptions);

            // Assert
            success.Should().BeTrue();

            DbContext.TravelPlans.Should().NotBeEmpty();

            travelPlan.CarId.Should().Be(travelPlanOptions.CarId);
            travelPlan.StartLocationId.Should().Be(travelPlanOptions.StartLocationId);
            travelPlan.EndLocationId.Should().Be(travelPlanOptions.EndLocationId);
            travelPlan.StartTimeUtc.Should().Be(travelPlanOptions.StartDateUtc);
            travelPlan.EndTimeUtc.Should().Be(travelPlanOptions.EndDateUtc);

            DbContext.TravelPlanEmployees.Select(tp => tp.EmployeeId).Should().BeEquivalentTo(employeeIds);

        }
        [Fact]
        public async Task CreateTravelPlan_WhenEmployeeNotAvailable_ReturnsSuccessFalse()
        {
            // Arrange 
           
            var carServiceStub = CarServiceIsCarAvailableReturns(true);

            var employeeServiceStub = EmployeeServiceReturnsEmployeesAvailable(false);

            var travelPlanService = TravelPlanServiceFactory(carServiceStub.Object, employeeServiceStub.Object);

            var travelPlanOptions = _fixture.Create<TravelPlanOptions>();

            var (success, message, travelPlan) = await travelPlanService.CreateTravelPlan(travelPlanOptions);
            success.Should().BeFalse();
            message?.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateTravelPlan_WhenNoCarsAvailable_ReturnsSuccessFalse()
        {
            // Arrange 

            var carServiceStub = CarServiceIsCarAvailableReturns(false);

            var employeeServiceStub = EmployeeServiceReturnsEmployeesAvailable(true);

            var travelPlanService = TravelPlanServiceFactory(carServiceStub.Object, employeeServiceStub.Object);

            var travelPlanOptions = _fixture.Create<TravelPlanOptions>();
            // Act
            var (success,message, travelPlan) = await travelPlanService.CreateTravelPlan(travelPlanOptions);

            // Assert
            success.Should().BeFalse();

            DbContext.TravelPlans.Should().BeEmpty();

        }

        [Theory]
        [InlineData(30)]
        [InlineData(15)]
        public async Task GetTravelPlansInDateRange_WhenTravelPlansInDateRange_ReturnsTravelPlans(int rangeInDays)
        {
            // Arrange 

            // travel lasted from 03.02 - 10.02 and current date is 25.02
            ITimeProvider currentDate = new CustomCurrentTimeProvider(new DateTime(2021, 02, 25, 8, 0,0));
            var travelStart = new DateTime(2021, 2, 3, 6, 0, 0);
            var travelEnd = new DateTime(2021, 2, 10, 16, 0,0);
            await AddTravelPlanToDatabase(travelStart, travelEnd);
            


            var carServiceStub = new Mock<ICarService>();
            var employeeServiceStub = new Mock<IEmployeeService>();
            var travelPlanService = TravelPlanServiceFactory(carServiceStub.Object, employeeServiceStub.Object, currentDate);
            
            // Act
            var travelPlansInDateRange =  travelPlanService.GetTravelPlansInDateRange(rangeInDays);

            // Assert
            PrepareForIncludeTest();
            
            travelPlansInDateRange.Should().NotBeEmpty();

            var travelPlan = travelPlansInDateRange.First();

            travelPlan.Car.Should().NotBeNull();
            travelPlan.TravelPlanEmployees.Should().NotBeEmpty();
            var employees = travelPlan.TravelPlanEmployees.Select(e => e.Employee);
            employees.Should().NotBeEmpty();
            travelPlan.StartLocation.Should().NotBeNull();
            travelPlan.EndLocation.Should().NotBeNull();

        }
        [Theory]
        [InlineData(1)]
        [InlineData(14)]
        public async Task GetTravelPlansInDateRange_WhenTravelPlansNotInDateRange_ReturnsEmpty(int rangeInDays)
        {
            // Arrange 

            // travel lasted from 03.02 - 10.02 and current date is 25.02
            ITimeProvider currentDate = new CustomCurrentTimeProvider(new DateTime(2021, 02, 25, 8, 0,0));
            var travelStart = new DateTime(2021, 2, 3, 6, 0, 0);
            var travelEnd = new DateTime(2021, 2, 10, 16, 0,0);
            await AddTravelPlanToDatabase(travelStart, travelEnd);

            var carServiceStub = new Mock<ICarService>();
            var employeeServiceStub = new Mock<IEmployeeService>();
            var travelPlanService = TravelPlanServiceFactory(carServiceStub.Object, employeeServiceStub.Object, currentDate);

            // Act
            var travelPlansInDateRange =  travelPlanService.GetTravelPlansInDateRange(rangeInDays);

            // Assert
            travelPlansInDateRange.Should().BeEmpty();
        }

        private async Task AddTravelPlanToDatabase(DateTime travelStart, DateTime travelEnd)
        {
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;
            await DbContext.AddRangeAsync(locations);

            var employees = SeedData.GenerateEmployees(10);

            var trip = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartTimeUtc = travelStart,
                EndTimeUtc = travelEnd,
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            var addEmployees = employees.Take(3).Select(e => new TravelPlanEmployees()
            {
                EmployeeId = e.EmployeeId,
            });

            addEmployees.ForEach(trip.TravelPlanEmployees.Add);

            await DbContext.Employees.AddRangeAsync(employees);

            await DbContext.AddRangeAsync(trip);

            await DbContext.SaveChangesAsync();
        }


        private static Mock<IEmployeeService> EmployeeServiceReturnsEmployeesAvailable(bool success)
        {
            var employeeServiceStub = new Mock<IEmployeeService>();
            employeeServiceStub
                .Setup(es =>
                    es.AreEmployeesAvailable(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((success, null));
            return employeeServiceStub;
        }

        private static Mock<ICarService> CarServiceIsCarAvailableReturns(bool isCarAvailable)
        {
            var carService = new Mock<ICarService>();
            carService.Setup(cs =>
                    cs.IsCarAvailable(It.IsAny<IsCarAvailableOptions>()))
                .ReturnsAsync((isCarAvailable, string.Empty));
            return carService;
        }

        private ITravelPlanService TravelPlanServiceFactory(ICarService carService, IEmployeeService employeeService) => TravelPlanServiceFactory(carService,employeeService, new SystemTimeProvider());

        private ITravelPlanService TravelPlanServiceFactory(ICarService carService, IEmployeeService employeeService,
            ITimeProvider timeProvider) =>
            new TravelPlanService(DbContext, carService, employeeService, timeProvider);
    }
}