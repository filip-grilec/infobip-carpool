using System.Collections.Generic;

namespace Carpool.Contracts.Response
{
    public class AvailableCarsResponse
    {
        public IEnumerable<CarDto> Cars { get; set; }
        public int Count { get; set; }
    }
}