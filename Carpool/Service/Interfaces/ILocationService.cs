using System.Collections.Generic;
using System.Threading.Tasks;
using Carpool.Data;

namespace Carpool.Service
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocations();
    }
}