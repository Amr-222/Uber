namespace Uber.PLL.Models
{
    public class DriverEdit
    {
        public int Id { get; set; } 
        public required string CarModel { get; set; }
        public required string CarType { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
