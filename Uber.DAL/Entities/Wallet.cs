using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uber.DAL.Entities
{
    public class Wallet
    {
        // IS IT NECESSARY? I think attribute in User & Driver is Enough ! 
        // We should make "Payment" Class instead of that


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public double Balance { get; private set; } = 0;

        public Wallet()
        {

        }

    }
}
