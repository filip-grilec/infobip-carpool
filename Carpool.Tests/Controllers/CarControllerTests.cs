using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Controllers;
using Carpool.Data;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace OkrApp.Tests.Controllers
{
    public class CarControllerTests
    {
        private Fixture _fixture = new Fixture();

        [Fact]
        public void GetAvailableCars_WhenCarsAvailable_ReturnsOk()
        {
            var cars = SeedData.GenerateCars(3);
            var carServiceStub = CarServiceReturns(cars);

            var getCarsRequest = _fixture.Create<AvailableCarsOptions>();
            MyMvc
                .Controller<CarController>(instance => instance
                    .WithDependencies(carServiceStub.Object)
                )
                .Calling(c => c.GetAvailableCars(getCarsRequest))
                .ShouldReturn()
                .Ok(result => result.WithModelOfType<AvailableCarsResponse>()
                    .Passing(model =>
                    {
                        model.Cars.Should().NotBeEmpty();
                        model.Count.Should().BePositive();
                    }));
        }
        [Fact]
        public void GetAvailableCars_WhenNoCarsAvailable_ReturnsNoContent()
        {
            var carServiceStub = CarServiceReturns(Enumerable.Empty<Car>());

            var getCarsRequest = _fixture.Create<AvailableCarsOptions>();
            MyMvc
                .Controller<CarController>(instance => instance
                    .WithDependencies(carServiceStub.Object)
                )
                .Calling(c => c.GetAvailableCars(getCarsRequest))
                .ShouldReturn()
                .NoContent();
        }

        private static Mock<ICarService> CarServiceReturns(IEnumerable<Car> cars)
        {
            var carServiceStub = new Mock<ICarService>();
            carServiceStub.Setup(cs => cs.GetAvailableCarsAsync(It.IsAny<AvailableCarsOptions>()))
                .ReturnsAsync((cars.Any(), cars));
            return carServiceStub;
        }
    }
}