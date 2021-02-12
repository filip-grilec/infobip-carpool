using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Data;
using Carpool.Service.Interfaces;
using Carpool.Time;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Service
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly CarpoolContext _dbContext;
        private readonly ICarService _carService;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeProvider _timeProvider;

        public TravelPlanService(CarpoolContext dbContext, ICarService carService, IEmployeeService employeeService,
            ITimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _carService = carService;
            _employeeService = employeeService;
            _timeProvider = timeProvider;
        }
        public async Task<(bool success, string? message, TravelPlan? travelPlan)> CreateTravelPlan(
            TravelPlanOptions travelPlanOptions)
        {

            var (success, carUnavailableMessage) = await _carService.IsCarAvailable(travelPlanOptions.Adapt<IsCarAvailableOptions>());

            if (!success)
            {
                return (false, carUnavailableMessage, null);
            }

            var (areEmployeesAvailable, errorMessage) = await _employeeService.AreEmployeesAvailable(travelPlanOptions.EmployeeIds,
                travelPlanOptions.StartDateUtc, travelPlanOptions.EndDateUtc);
            
            if (!areEmployeesAvailable)
            {
                return (false, errorMessage, null);
            }
            
            var travelPlan = new TravelPlan()
            {
                CarId = travelPlanOptions.CarId,
                StartLocationId = travelPlanOptions.StartLocationId,
                EndLocationId = travelPlanOptions.EndLocationId,
                StartTimeUtc = travelPlanOptions.StartDateUtc,
                EndTimeUtc = travelPlanOptions.EndDateUtc,
                TravelPlanEmployees = new List<TravelPlanEmployees>()
            };

            foreach (var employeeId in travelPlanOptions.EmployeeIds)
            {
                travelPlan.TravelPlanEmployees.Add(new TravelPlanEmployees()
                {
                    EmployeeId = employeeId,
                });
            }


            await _dbContext.TravelPlans.AddAsync(travelPlan);

            await _dbContext.SaveChangesAsync();
            
            return (true, null, travelPlan);
        }

        public IQueryable<TravelPlan> GetTravelPlansInDateRange(TimeSpan timeSpan)
        {
            var timeTreshold = _timeProvider.NowUtc() - timeSpan;
            var dayTreshold = timeTreshold.Date;
            var travelPlans = _dbContext.TravelPlans
                .Include(tp => tp.Car)
                .Include(tp => tp.StartLocation)
                .Include(tp => tp.EndLocation)
                .Include(tp => tp.TravelPlanEmployees)
                .ThenInclude(tpe => tpe.Employee)
                .Where(tp => tp.EndTimeUtc > dayTreshold);
            return travelPlans;
        }

        public IQueryable<TravelPlan> GetTravelPlansInDateRange(int rangeInDays) => GetTravelPlansInDateRange(TimeSpan.FromDays(rangeInDays));
    }
}