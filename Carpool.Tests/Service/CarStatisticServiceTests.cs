using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using Carpool.Service.Interfaces;
using FluentAssertions;
using Moq;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using Xunit;

namespace OkrApp.Tests.Service
{
    public class CarStatisticServiceTests : TestWithInMemoryDatabase
    {
        private Fixture _fixture = new Fixture();

        public CarStatisticServiceTests()
        {
            ClearDatabaseForConcurrencyReasons();
        }
        [Fact]
        public async Task GetStatistics_WhenStatisticsInLastMonth_ReturnsStatistics()
        {
            // Arrange
            var (travelPlan, employee, car) = GenerateTravelPlan();

            var travelPlanList = new List<TravelPlan>() {travelPlan}.AsQueryable();

            var travelPlanServiceStub = new Mock<ITravelPlanService>();
            travelPlanServiceStub.Setup(tps => tps.GetTravelPlansInDateRange(It.IsAny<int>())).Returns(travelPlanList);
            

            var carStatisticService = CarStatisticServiceFactory(travelPlanServiceStub.Object);
            // Act

            var statisticResponses = await carStatisticService.GetCarStatisticsInPreviousDays(_fixture.Create<int>());
            // Assert

            statisticResponses.Should().NotBeEmpty();
        }

        private (TravelPlan, Employee, Car) GenerateTravelPlan()
        {
            var travelPlan = _fixture.Build<TravelPlan>()
                .Without(tp => tp.TravelPlanEmployees)
                .Create();

            var employee = _fixture.Build<Employee>()
                .Without(e => e.TravelPlanEmployees)
                .Create();

            var travelPlanEmployees = new List<TravelPlanEmployees>()
            {
                new TravelPlanEmployees()
                {
                    Employee = employee,
                    EmployeeId = employee.EmployeeId,
                    TravelPlanId = travelPlan.TravelPlanId
                }
            };

            travelPlan.TravelPlanEmployees = travelPlanEmployees;
            return (travelPlan, employee, travelPlan.Car);
        }

        private ICarStatisticsService CarStatisticServiceFactory(ITravelPlanService travelPlanService) => new CarStatisticService(travelPlanService);
    }
}