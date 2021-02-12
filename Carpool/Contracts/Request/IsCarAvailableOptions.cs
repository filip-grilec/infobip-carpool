using System;

namespace Carpool.Contracts.Request
{
    public class IsCarAvailableOptions
    {
        public int CarId { get; set; }
        public int StartLocationId { get;  set; }
        public int EndLocationId { get;  set; }
        public DateTime StartDateUtc { get;  set; }
        public DateTime EndDateUtc { get;  set; }
        public int EmployeesOnTrip { get; set; }
    }
}