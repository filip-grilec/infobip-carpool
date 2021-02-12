using System.Collections.Generic;

namespace Carpool.Contracts.Response
{
    public class CarStatistic
    {
        public CarDto Car { get; set; }

        public List<TravelPlanStatistic> TravelPlanStatistics { get; set; } = new List<TravelPlanStatistic>();
    }
}