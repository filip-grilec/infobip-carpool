using System;
using System.Collections.Generic;

namespace Carpool.Data
{
    public class TravelPlan
    {
        public int TravelPlanId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int StartLocationId { get; set; }
        public Location StartLocation { get; set; }
        public int EndLocationId { get; set; }
        public Location EndLocation { get; set; }

        public ICollection<TravelPlanEmployees> TravelPlanEmployees { get; set; }
    }
}