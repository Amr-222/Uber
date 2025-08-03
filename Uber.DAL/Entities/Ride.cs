namespace Uber.DAL.Entities
{
    public class Ride
    {
        public int Id { get; private set; }
        public Driver Driver { get; private set; }
        public Customer Customer { get; private set; }
        public double Price { get; private set; }
        public double Distance { get; private set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public bool IsCanceled { get; private set; } = false;
        // Maybe add the two data members for the starting and ending locations
        // after knowing how to use the maps api
        public Ride(Customer customer, Driver driver, double price, double distance) // change the constractor if you change the data members
        {
            Customer = customer;
            Driver = driver;
            Price = price;
            Distance = distance;
        }
        public (bool, string?) CancelRide()
        {
            try
            {
                IsCanceled = true;
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
