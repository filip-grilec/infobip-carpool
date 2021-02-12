using AutoFixture;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Controllers;
using Carpool.Data;
using Carpool.Service;
using Carpool.Service.Interfaces;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace OkrApp.Tests.Controllers
{
    public class TravelPlanControllerTests
    {
        private Fixture _fixture = new Fixture();

        [Fact]
        public void CreateTravelPlan_WhenCreatedReturns_Ok()
        {
            var request = _fixture.Create<CreateTravelPlanRequest>();
            var travelPlanServiceMock = new Mock<ITravelPlanService>();
            var travelPlan = new TravelPlan() {CarId = 1, StartLocationId = 1, EndLocationId = 1};
            travelPlanServiceMock.Setup(tps => tps.CreateTravelPlan(It.IsAny<TravelPlanOptions>()))
                .ReturnsAsync((true, null, travelPlan));

            MyMvc
                .Controller<TravelPlanController>(instance => instance
                    .WithDependencies(travelPlanServiceMock.Object)
                )
                .Calling(c => c.CreateTravelPlan(request))
                .ShouldReturn()
                .Ok(result => result.WithModelOfType<TravelPlanDto>());
        }

        [Fact]
        public void CreateTravelPlan_WhenCantCreate_ReturnsBadRequestWithMessage()
        {
            var request = _fixture.Create<CreateTravelPlanRequest>();
            var travelPlanServiceMock = new Mock<ITravelPlanService>();
            travelPlanServiceMock.Setup(tps => tps.CreateTravelPlan(It.IsAny<TravelPlanOptions>()))
                .ReturnsAsync((false, "ErrorMessage", null));
            MyMvc
                .Controller<TravelPlanController>(instance => instance
                    .WithDependencies(travelPlanServiceMock.Object)
                )
                .Calling(c => c.CreateTravelPlan(request))
                .ShouldReturn()
                .BadRequest();
        }

    }
}