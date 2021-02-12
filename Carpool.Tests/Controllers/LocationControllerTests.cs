using Carpool.Contracts.Response;
using Carpool.Controllers;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace OkrApp.Tests.Controllers
{
    public class LocationControllerTests
    {
        [Fact]
        public void GetLocations_WhenLocations_ReturnsOk()
        {
            var locationServiceMock = new Mock<ILocationService>();

            locationServiceMock.Setup(ls => ls.GetAllLocations())
                .ReturnsAsync(SeedData.GenerateLocations(2));

            MyMvc
                .Controller<LocationController>(instance => instance
                    .WithDependencies(locationServiceMock.Object)
                )
                .Calling(c => c.GetLocations())
                .ShouldReturn()
                .Ok(result => result.WithModelOfType<LocationsResponse>()
                    .Passing(model =>
                    {
                        model.Locations.Should().HaveCount(2);
                    }));
        }
    }
}