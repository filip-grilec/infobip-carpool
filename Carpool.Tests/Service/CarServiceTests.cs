using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using Carpool.Contracts.Request;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace OkrApp.Tests
{
    public class CarServiceTests : TestWithInMemoryDatabase
    {
        public CarServiceTests()
        {
            ClearDatabaseForConcurrencyReasons();
        }

        [Fact]
        public async Task GetAvailableCars_WhenAvailableCarAtLocation_ReturnsAvailableCars()
        {
            // Arrange 

            var cars = SeedData.GenerateCars(2);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];
            var car2 = cars[1];

            var locations = SeedData.GenerateLocations(2);
            var locationA = locations[0];
            var locationB = locations[1];
            await DbContext.AddRangeAsync(locations);

            // Booked from 3.2 - 10.2 , Free from 10.2 - 20.2, Booked from 20.0 - 1.3
            var bookedFromAToB = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationA.LocationId,
                EndLocationId = locationB.LocationId,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10)
            };

            var bookedFromBToA = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationB.LocationId,
                EndLocationId = locationA.LocationId,
                StartTimeUtc = new DateTime(2021, 2, 20),
                EndTimeUtc = new DateTime(2021, 3, 1)
            };

            // Car 2 starting location and available all the time
            var car2StartLocation = new TravelPlan()
            {
                CarId = car2.CarId,
                StartLocationId = locationB.LocationId,
                EndLocationId = locationB.LocationId,
                StartTimeUtc = new DateTime(2020, 1, 1),
                EndTimeUtc = new DateTime(2020, 1, 2)
            };

            await DbContext.TravelPlans.AddRangeAsync(bookedFromAToB, bookedFromBToA, car2StartLocation);

            await DbContext.SaveChangesAsync();

            var carService = CarServiceFactory();

            var timeWhenCarShouldBeAvailable = new DateTime(2021, 2, 11);

            var carOptions = new AvailableCarsOptions()
            {
                StartDateUtc = timeWhenCarShouldBeAvailable,
                EndDateUtc = timeWhenCarShouldBeAvailable.AddDays(7),
                StartLocationId = locationB.LocationId,
                EndLocationId = locationB.LocationId
            };
            // Act
            var (success, resultCars) = await carService.GetAvailableCarsAsync(carOptions);

            // Assert
            success.Should().BeTrue();

            resultCars.Should().NotBeEmpty();
            resultCars.Should().HaveCount(cars.Count);
        }

        private ICarService CarServiceFactory() => new CarService(DbContext);

        [Fact(Skip = "Descoped and not implemented yet")]
        public async Task GetAvailableCars_WhenCarNotAtLocation_DoesntReturnCar()
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            // Car available at location B
            var previousTravelPlan = new TravelPlan()
            {
                StartLocationId = locationB,
                EndLocationId = locationB,
                CarId = car1.CarId,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10)
            };

            // Car needs to be at location B at this date 
            var travelPlanInTheFuture = new TravelPlan()
            {
                CarId = car1.CarId,
                StartLocationId = locationB,
                EndLocationId = locationB,
                StartTimeUtc = new DateTime(2021, 2, 20),
                EndTimeUtc = new DateTime(2021, 3, 1)
            };

            await DbContext.TravelPlans.AddRangeAsync(previousTravelPlan, travelPlanInTheFuture);

            await DbContext.SaveChangesAsync();

            var carService = CarServiceFactory();

            var timeWhenCarShouldBeAvailable = new DateTime(2021, 2, 11);

            // Find cars for location B
            var carOptions = new AvailableCarsOptions()
            {
                StartLocationId = locationB,
                EndLocationId = locationA,
                StartDateUtc = timeWhenCarShouldBeAvailable,
                EndDateUtc = timeWhenCarShouldBeAvailable.AddDays(7),
            };
            // Act
            var (success, resultCars) = await carService.GetAvailableCarsAsync(carOptions);

            // Assert
            success.Should().BeFalse();

            resultCars.Should().BeEmpty();
        }


        [Fact(Skip = "Descoped and not implemented yet")]
        public async Task GetAvailableCars_WhenCarBookedInFutureAtLocation_DoesntReturnCar()
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            // Car available at location B
            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            var previousTravelPlan = new TravelPlan()
            {
                StartLocationId = locationA,
                EndLocationId = locationB,
                CarId = car1.CarId,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10)
            };

            await DbContext.TravelPlans.AddRangeAsync(previousTravelPlan);

            await DbContext.SaveChangesAsync();

            var carService = CarServiceFactory();

            var timeWhenCarShouldBeAvailable = new DateTime(2021, 2, 11);

            // Find cars for location A
            var carOptions = new AvailableCarsOptions()
            {
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartDateUtc = timeWhenCarShouldBeAvailable,
                EndDateUtc = timeWhenCarShouldBeAvailable.AddDays(7),
            };
            // Act
            var (success, resultCars) = await carService.GetAvailableCarsAsync(carOptions);

            // Assert
            success.Should().BeFalse();

            resultCars.Should().BeEmpty();
        }

        [Theory]
        // All unavailable dates
        [InlineData("2021-02-01", "2021-02-8")]
        [InlineData("2021-02-04", "2021-02-11")]
        [InlineData("2021-02-09", "2021-02-20")]
        [InlineData("2021-02-05", "2021-02-8")]
        [InlineData("2021-02-01", "2021-03-1")]
        public async Task GetAvailableCars_WhenCarBooked_DoesntReturnCar(string tripStartString, string tripEndString)
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            var previousTravelPlan = new TravelPlan()
            {
                StartLocationId = locationA,
                EndLocationId = locationB,
                CarId = car1.CarId,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10)
            };

            await DbContext.TravelPlans.AddRangeAsync(previousTravelPlan);

            await DbContext.SaveChangesAsync();

            var carService = CarServiceFactory();

            var carOptions = new AvailableCarsOptions()
            {
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartDateUtc = DateTime.Parse(tripStartString),
                EndDateUtc = DateTime.Parse(tripEndString),
            };
            // Act
            var (success, resultCars) = await carService.GetAvailableCarsAsync(carOptions);

            // Assert
            success.Should().BeFalse();

            resultCars.Should().BeEmpty();
        }

        [Fact]
        public async Task IsCarAvailable_WhenCarNotAvailable_ReturnsFalseWithErrorMessage()
        {
            // Arrange 
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            var car1 = cars[0];

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            var previousTravelPlan = new TravelPlan()
            {
                StartLocationId = locationA,
                EndLocationId = locationB,
                CarId = car1.CarId,
                StartTimeUtc = new DateTime(2021, 2, 3),
                EndTimeUtc = new DateTime(2021, 2, 10)
            };

            await DbContext.TravelPlans.AddRangeAsync(previousTravelPlan);

            await DbContext.SaveChangesAsync();

            var carService = CarServiceFactory();

            var carOptions = new IsCarAvailableOptions()
            {
                CarId = car1.CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartDateUtc = DateTime.Parse("2021-02-01"),
                EndDateUtc = DateTime.Parse("2021-03-1"),
            };
            // Act
            var (success, errorMessage) = await carService.IsCarAvailable(carOptions);

            // Assert
            success.Should().BeFalse();

            errorMessage.Should().NotBeEmpty();
        }

        [Fact]
        public async Task IsCarAvailable_WhenCarAvailable_ReturnsSuccessTrue()
        {
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            await DbContext.SaveChangesAsync();

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            var carService = CarServiceFactory();

            var carOptions = new IsCarAvailableOptions()
            {
                CarId = cars.First().CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartDateUtc = DateTime.Parse("2021-02-01"),
                EndDateUtc = DateTime.Parse("2021-03-1"),
            };

            var (success, errorMessage) = await carService.IsCarAvailable(carOptions);

            success.Should().BeTrue();
        }

        [Fact]
        public async Task IsCarAvailable_WhenMoreEmployeesThanSeatsInCar_ReturnsSuccessFalse()
        {
            var cars = SeedData.GenerateCars(1);
            await DbContext.AddRangeAsync(cars);

            await DbContext.SaveChangesAsync();

            var locations = SeedData.GenerateLocations(2);
            await DbContext.AddRangeAsync(locations);

            var locationA = locations[0].LocationId;
            var locationB = locations[1].LocationId;

            var carService = CarServiceFactory();

            int moreThanAvailableInCar = 7;
            var carOptions = new IsCarAvailableOptions()
            {
                CarId = cars.First().CarId,
                StartLocationId = locationA,
                EndLocationId = locationB,
                StartDateUtc = DateTime.Parse("2021-02-01"),
                EndDateUtc = DateTime.Parse("2021-03-1"),
                EmployeesOnTrip = moreThanAvailableInCar
            };

            var (success, errorMessage) = await carService.IsCarAvailable(carOptions);

            success.Should().BeFalse();
            errorMessage.Should().NotBeEmpty();
        }
    }
}