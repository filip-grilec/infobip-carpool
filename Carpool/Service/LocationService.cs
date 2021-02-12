using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Data;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Service
{
    public class LocationService : ILocationService
    {
        private readonly CarpoolContext _dbContext;

        public LocationService(CarpoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Location>> GetAllLocations() => await _dbContext.Locations.ToListAsync();
    }
}