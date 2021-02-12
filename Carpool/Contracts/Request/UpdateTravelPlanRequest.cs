using System;
using System.Collections.Generic;

namespace Carpool.Contracts.Request
{
    public class UpdateTravelPlanRequest
    {
        public DateTime StartDateUtc { get;  set; }
        public DateTime EndDateUtc { get;  set; }
        public int CarId { get;  set; }
        public int StartLocationId { get;  set; }
        public int EndLocationId { get;  set; }
        public IEnumerable<int> EmployeeIds { get; set; }
    }
}