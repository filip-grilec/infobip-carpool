using System.Threading.Tasks;
using Carpool.Database;
using Carpool.Service;
using FluentAssertions;
using Xunit;

namespace OkrApp.Tests
{
    public class LocationServiceTests : TestWithInMemoryDatabase
    {

        public LocationServiceTests()
        {
            ClearDatabaseForConcurrencyReasons();
        }
        [Fact]
        public async Task GetLocations_ReturnsLocations()
        {
            DbContext.Locations.AddRange(SeedData.GenerateLocations(2));
            DbContext.SaveChanges();

            var locationService = LocationServiceFactory();

            var allLocations = await locationService.GetAllLocations();

            allLocations.Should().HaveCount(2);
        }

        private LocationService LocationServiceFactory() => new LocationService(DbContext);
    }
}