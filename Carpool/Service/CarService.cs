using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Carpool.Service
{
    public class CarService : ICarService
    {
        private const string CarUnavailableMessage = "This car is unavailable, please select another car.";
        private const string NotEnoughRoomMessage = "There are more employees than seats in the car";
        private readonly CarpoolContext _dbContext;

        public CarService(CarpoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool success, IEnumerable<Car> cars)> GetAvailableCarsAsync(
            AvailableCarsOptions availableCarsOptions)
        {
            var tripStart = availableCarsOptions.StartDateUtc;
            var tripEnd = availableCarsOptions.EndDateUtc;


            var unavailableCars =
                _dbContext.TravelPlans.Where(tp => (tripStart >= tp.StartTimeUtc && tripStart <= tp.EndTimeUtc)
                                                   || (tripEnd >= tp.StartTimeUtc && tripEnd <= tp.EndTimeUtc)
                                                   || (tripStart <= tp.StartTimeUtc && tripEnd >= tp.EndTimeUtc));


            var unavailableCarIds = unavailableCars.Select(c => c.CarId);

            var availableCars = _dbContext.Cars.Where(c => !unavailableCarIds.Contains(c.CarId));
            return (await availableCars.AnyAsync(), await availableCars.ToListAsync());
        }

        public async Task<(bool success, string message)> IsCarAvailable(IsCarAvailableOptions isCarAvailableOptions)
        {
            var (success, cars) = await GetAvailableCarsAsync(isCarAvailableOptions.Adapt<AvailableCarsOptions>());

            var car = cars.FirstOrDefault(c => c.CarId == isCarAvailableOptions.CarId);

            if (car == null)
            {
                return (false, CarUnavailableMessage);
            }

            var employeesOnTrip = isCarAvailableOptions.EmployeesOnTrip;
            
            if (employeesOnTrip > car.Seats)
            {
                return (false, NotEnoughRoomMessage);
            }

            return (true, string.Empty);
        }
    }
}