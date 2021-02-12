using System.Collections.Generic;

namespace Carpool.Contracts.Response
{
    public class LocationsResponse
    {
        public IEnumerable<LocationDto> Locations { get; set; }
    }
}