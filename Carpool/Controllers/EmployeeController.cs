using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Request;
using Carpool.Contracts.Response;
using Carpool.Service;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carpool.Controllers
{
    [ApiController]
    [Route("api/employees")]
    // [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] AvailableEmployeesRequest request)
        {
            var (success, employees) = await _employeeService.GetAvailableEmployees(request.StartDateUtc, request.EndDateUtc);
            return success ? (IActionResult) Ok(employees.Adapt<IEnumerable<EmployeeDto>>()) : NoContent();
        }
    }
}