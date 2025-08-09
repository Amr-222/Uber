namespace Uber.DAL.Entities
{
    public class RideRequestController
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; 
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
