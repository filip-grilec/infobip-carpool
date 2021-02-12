namespace Carpool.Contracts.Response
{
    public class EmployeeDto
    {
        public int  EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool HasDriverLicence { get; set; }
        public string? Avatar { get; set; }
    }
}