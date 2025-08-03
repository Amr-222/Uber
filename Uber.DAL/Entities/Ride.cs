using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uber.DAL.Entities
{   public class Ride
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }

      

        public Location From { get; set; }
        public Location To { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int Rate {  get; set; }









    }
}
