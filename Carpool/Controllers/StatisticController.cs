using System.Linq;
using System.Threading.Tasks;
using Carpool.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Carpool.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private readonly ICarStatisticsService _statisticsService;

        public StatisticController(ICarStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatisticsForPeriod(int days  = 30)
        {
            var carStatisticsInPreviousDays = (await _statisticsService.GetCarStatisticsInPreviousDays(days))
                .ToList();

            bool hasStatistics = carStatisticsInPreviousDays.Any();
            return hasStatistics ? (IActionResult) Ok(carStatisticsInPreviousDays.ToList()) : NoContent();
        }
    }
}