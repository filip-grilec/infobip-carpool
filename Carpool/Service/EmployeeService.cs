using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Carpool.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Carpool.Service
{
    public class EmployeeService : IEmployeeService
    {
        private const string NoDriverLicenceMessage = "None of the employees has a driver licence, please select at least one driver with a driver licence";
        private const string EmployeesAreOnAnotherTripMessage = "Employees are on another trip, please select other employees";
        private readonly CarpoolContext _dbContext;

        public EmployeeService(CarpoolContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool success, string? errorMessage)> AreEmployeesAvailable(IEnumerable<int> employeeIds,
            DateTime tripStart, DateTime tripEnd)
        {
            var employeeTravelPlans = _dbContext.TravelPlanEmployees
                .Include(tpe => tpe.TravelPlan)
                .Where(tpe => employeeIds.Contains(tpe.EmployeeId));

            var travelPlansDuringTheTrip = TravelPlansDuringTheTrip(tripStart, tripEnd, employeeTravelPlans);

            if (await travelPlansDuringTheTrip.AnyAsync())
            {
                return (false, EmployeesAreOnAnotherTripMessage);
            }

            var employees = _dbContext.Employees.Where(e => employeeIds.Contains(e.EmployeeId));

            bool oneDriverHasDriverLicence = await employees.AnyAsync(e => e.HasDriverLicence);
            if (!oneDriverHasDriverLicence)
            {
                return (false, NoDriverLicenceMessage);
            }
            
            return (true, null);
        }

        public async Task<(bool success, IEnumerable<Employee> employees)> GetAvailableEmployees(DateTime startDate, DateTime endDate)
        {
            var travelPlans = _dbContext.TravelPlanEmployees.Include(tpe => tpe.TravelPlan);

            var unavailableEmployees = TravelPlansDuringTheTrip(startDate, endDate, travelPlans);

            var unavailableEmployeeIds = unavailableEmployees.Select(tpe => tpe.EmployeeId);
            var availableEmployees = _dbContext.Employees.Where(e => !unavailableEmployeeIds.Contains(e.EmployeeId));
            
            return (await availableEmployees.AnyAsync(),await availableEmployees.ToListAsync());
        }

        private static IQueryable<TravelPlanEmployees> TravelPlansDuringTheTrip(DateTime tripStart, DateTime tripEnd, IQueryable<TravelPlanEmployees> employeeTravelPlans)
        {
            return employeeTravelPlans.Where(tp =>
                (tripStart >= tp.TravelPlan.StartTimeUtc && tripStart <= tp.TravelPlan.EndTimeUtc)
                || (tripEnd >= tp.TravelPlan.StartTimeUtc && tripEnd <= tp.TravelPlan.EndTimeUtc)
                || (tripStart <= tp.TravelPlan.StartTimeUtc && tripEnd >= tp.TravelPlan.EndTimeUtc));
        }
    }
}