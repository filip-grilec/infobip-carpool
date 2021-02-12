using System;

namespace Carpool.Contracts.Response
{
    public class TravelPlanDto
    {
        public int TravelPlanId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public CarDto Car { get; set; }
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
    }
}