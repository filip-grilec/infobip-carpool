using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpool.Data;

namespace Carpool.Service
{
    public interface IEmployeeService
    {
        Task<(bool success, string? errorMessage)> AreEmployeesAvailable(IEnumerable<int> employeeIds,
            DateTime tripStart, DateTime tripEnd);

        Task<(bool success, IEnumerable<Employee> employees)> GetAvailableEmployees(DateTime startDate, DateTime endDate);
    }
}