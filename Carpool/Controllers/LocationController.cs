using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Response;
using Carpool.Service;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Controllers
{
    [ApiController]
    [Route("api/locations")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _locationService.GetAllLocations();
            var response = new LocationsResponse()
            {
                Locations = locations.Adapt<List<LocationDto>>()
            };
            
            return Ok(response);
        }
    }
}