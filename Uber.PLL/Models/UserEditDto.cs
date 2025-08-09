namespace Uber.PLL.Models
{
    public class UserEditDto
    {
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
    }

}
