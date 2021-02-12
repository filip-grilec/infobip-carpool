using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus.Extensions;
using Carpool.Contracts.Response;
using Carpool.Data;
using Carpool.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Carpool.Service
{
    public class CarStatisticService : ICarStatisticsService
    {
        private readonly ITravelPlanService _travelPlanService;

        public CarStatisticService(ITravelPlanService travelPlanService)
        {
            _travelPlanService = travelPlanService;
        }

        public async Task<IEnumerable<CarStatistic>> GetCarStatisticsInPreviousDays(int days)
        {
            var travelPlans = _travelPlanService.GetTravelPlansInDateRange(days);

            var travelPlansGroupedByCar = travelPlans.ToList()
                .GroupBy(tp => new {tp.CarId, tp.Car}, (g, tp) => new
                {
                    g.Car.CarId,
                    g.Car,
                    Trips = tp.Select(x => new
                    {
                        x.TravelPlanId,
                        StartLocation = x.StartLocation.Name,
                        EndLcoation = x.EndLocation.Name,
                        StartDate = x.StartTimeUtc,
                        EndDate = x.EndTimeUtc,
                        Employees = x.TravelPlanEmployees.Select(tpe => tpe.Employee)
                    })
                }).ToList();

            var carStatistics = new List<CarStatistic>();

            foreach (var car in travelPlansGroupedByCar)
            {
                var carStatistic = new CarStatistic()
                {
                    Car = car.Car.Adapt<CarDto>(),
                };

                var travelPlanStatistics = car.Trips.Select(t => new TravelPlanStatistic()
                {
                    TravelPlanId = t.TravelPlanId,
                    Employees = MapEmployee(t.Employees),
                    StartLocation = t.StartLocation,
                    EndLocation = t.EndLcoation,
                    StartTimeUtc = t.StartDate,
                    EndTimeUtc = t.EndDate
                });
                carStatistic.TravelPlanStatistics.AddRange(travelPlanStatistics);

                carStatistics.Add(carStatistic);
            }

            return carStatistics;
        }

        private IEnumerable<EmployeeDto> MapEmployee(IEnumerable<Employee> employees)
        {
            var mappedEmployees = employees.Adapt<List<EmployeeDto>>();

            foreach (var employeeDto in mappedEmployees)
            {
                employeeDto.Avatar = employeeDto.EmployeeName.FirstOrDefault().ToString();
            }

            return mappedEmployees;
        }
    }
}