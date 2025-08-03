using Microsoft.EntityFrameworkCore.Storage;

namespace Uber.DAL.Entities
{
    public class Wallet
    {
        public double Balance { get; private set; }
        public DateTime? ModifiedAt { get; private set; }

        public Wallet() 
        {
            Balance = 0;
        }
        public (bool, string?) HasEnoughMoney(double amount)
        {
            try
            {
                return (amount <= Balance, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool, string?) AddToBalance(double amount)
        {
            try
            {
                if (amount > 0)
                {
                    Balance += amount;
                    return (true, null);
                }
                return (false, "the amount to be added to Balance in Wallet is Negative");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
