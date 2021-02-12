using System;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Data;

namespace Carpool.Service.Interfaces
{
    public interface ITravelPlanService
    {
        Task<(bool success, string? message, TravelPlan? travelPlan)> CreateTravelPlan(
            TravelPlanOptions travelPlanOptions);

        IQueryable<TravelPlan> GetTravelPlansInDateRange(TimeSpan timeSpan);
        IQueryable<TravelPlan> GetTravelPlansInDateRange(int rangeInDays);
    }
}