namespace Uber.DAL.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public User user { get; set; }
        public Wallet()
        {
            Balance = 0.0m; 
        }
    }
}
