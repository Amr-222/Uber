using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uber.DAL.Entities
{   public class Ride
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

      

        //public Location From { get; set; }
        //public Location To { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int Rate {  get; set; }
        
        public string DriverId { get; set; }
        public Driver Driver { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }








    }
}
