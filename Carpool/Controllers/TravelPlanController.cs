using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Service;
using Carpool.Service.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpool.Controllers
{
    [ApiController]
    [Route("api/travelplan")]
    [Authorize]
    public class TravelPlanController : ControllerBase
    {
        private readonly ITravelPlanService _travelPlanService;

        public TravelPlanController(ITravelPlanService travelPlanService)
        {
            _travelPlanService = travelPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTravelPlan([FromBody] CreateTravelPlanRequest request)
        {
            var (success, message, travelPlan) = await _travelPlanService.CreateTravelPlan(request.Adapt<TravelPlanOptions>());
            
            return success ? (IActionResult) Ok(travelPlan?.Adapt<TravelPlanDto>()) : BadRequest(message);
        }
        
    }
}