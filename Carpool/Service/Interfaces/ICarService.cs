using System.Collections.Generic;
using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Data;

namespace Carpool.Service
{
    public interface ICarService
    {
        Task<(bool success, IEnumerable<Car> cars)> GetAvailableCarsAsync(AvailableCarsOptions availableCarsOptions);
        Task<(bool success, string message)> IsCarAvailable(IsCarAvailableOptions isCarAvailableOptions);
    }
}