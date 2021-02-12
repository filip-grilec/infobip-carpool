using System;

namespace Carpool.Contracts.Request
{
    public class AvailableEmployeesRequest
    {
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
    }
}