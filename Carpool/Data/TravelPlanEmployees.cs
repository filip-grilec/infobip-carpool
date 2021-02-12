namespace Carpool.Data
{
    public class TravelPlanEmployees
    {
        public int TravelPlanEmployeesId { get; set; }
        public int TravelPlanId { get; set; }
        public TravelPlan TravelPlan { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}