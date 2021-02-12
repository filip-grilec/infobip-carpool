namespace Carpool.Contracts.Response
{
    public class CarDto
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public string CarType { get; set; }
        public string Color { get; set; }
        public string LicencePlate { get; set; }
        public int Seats { get; set; }
    }
}