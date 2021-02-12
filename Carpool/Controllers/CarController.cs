using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Service;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Controllers
{
    [ApiController]
    [Route("api/cars")]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableCars(AvailableCarsOptions options)
        {
            var (success, cars) = await _carService.GetAvailableCarsAsync(options);

            if (!success)
            {
                return NoContent();
            }

            var response = new AvailableCarsResponse()
            {
                Cars = cars.Adapt<List<CarDto>>(),
                Count = cars.Count()
            };

            return Ok(response);
        }
    }
}