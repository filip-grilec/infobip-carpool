using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Response;

namespace Carpool.Service.Interfaces
{
    public interface ICarStatisticsService
    {
        Task<IEnumerable<CarStatistic>> GetCarStatisticsInPreviousDays(int days);
    }
}