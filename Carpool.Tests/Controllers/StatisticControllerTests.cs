using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Carpool.Contracts.Response;
using Carpool.Controllers;
using Carpool.Service;
using Carpool.Service.Interfaces;
using FluentAssertions;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace OkrApp.Tests.Controllers
{
    public class StatisticControllerTests
    {

        private Fixture _fixture = new Fixture();

        [Fact]
        public void GetStatistics_WhenStatisticsPresent_ReturnsStatistics()
        {

            // Arrange
            var statistics = _fixture.Create<List<CarStatistic>>();

            var statisticServiceStub = new Mock<ICarStatisticsService>();
            statisticServiceStub.Setup(cs => cs.GetCarStatisticsInPreviousDays(It.IsAny<int>()))
                .ReturnsAsync(statistics);

            MyMvc
                .Controller<StatisticController>(instance => instance
                    .WithDependencies(statisticServiceStub.Object)
                )
                // Act
                .Calling(c => c.GetStatisticsForPeriod(_fixture.Create<int>()))
                .ShouldReturn()
                // Assert
                .Ok(result => result.WithModelOfType<IEnumerable<CarStatistic>>()
                    .Passing(model =>
                    {
                        model.Count().Should().Be(statistics.Count);
                    }));

        }
        [Fact]
        public void GetStatistics_WhenNoStatistics_ReturnsNoContent()
        {
            // Arrange
            var statisticServiceStub = new Mock<ICarStatisticsService>();
            statisticServiceStub.Setup(cs => cs.GetCarStatisticsInPreviousDays(It.IsAny<int>()))
                .ReturnsAsync(Enumerable.Empty<CarStatistic>());

            MyMvc
                .Controller<StatisticController>(instance => instance
                    .WithDependencies(statisticServiceStub.Object)
                )
                // Act
                .Calling(c => c.GetStatisticsForPeriod(_fixture.Create<int>()))
                .ShouldReturn()
                // Assert
                .NoContent();

        }
        
        
    }
}