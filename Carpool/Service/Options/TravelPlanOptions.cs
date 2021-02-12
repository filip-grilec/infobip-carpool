using System;
using System.Collections.Generic;

namespace Carpool.Service
{
    public class TravelPlanOptions
    {
        public DateTime StartDateUtc { get;  set; }
        public DateTime EndDateUtc { get;  set; }
        public int CarId { get;  set; }
        public int StartLocationId { get;  set; }
        public int EndLocationId { get;  set; }
        public IEnumerable<int> EmployeeIds { get; set; }
    }
}