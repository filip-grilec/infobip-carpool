using System;
using System.Collections.Generic;

namespace Carpool.Contracts.Response
{
    public class TravelPlanStatistic
    {
        public int TravelPlanId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

        public IEnumerable<EmployeeDto> Employees { get; set; }
    }
}