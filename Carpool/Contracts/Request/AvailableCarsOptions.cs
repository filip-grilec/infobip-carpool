using System;

namespace Carpool.Contracts.Request
{
    public class AvailableCarsOptions
    {
        public int StartLocationId { get;  set; }
        public int EndLocationId { get;  set; }
        public DateTime StartDateUtc { get;  set; }
        public DateTime EndDateUtc { get;  set; }
    }
}