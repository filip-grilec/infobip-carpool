using System.Collections.Generic;

namespace Carpool.Data
{
    public class Employee
    {
        public int  EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool HasDriverLicence { get; set; }

        public ICollection<TravelPlanEmployees> TravelPlanEmployees { get; set; }
    }
}