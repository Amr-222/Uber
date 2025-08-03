using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class Driver: ApplicationUser
    {
        //License  **

        public DateTime? ModifiedAt { get; private set; }
    

        public double Rate { get; private set; }

        public Driver() { }

        public Driver(string name, Gender gender, int age, DateTime DateOfBirth, string? ImagePath, string street, string city)
        {
            this.Name = name;
            this.Gender = gender;
            this.Age = age;
            this.DateOfBirth = DateOfBirth;
            this.ImagePath = ImagePath;
            this.Address = new Location { Street = street, City = city };

        }

        //Navigation Property


        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }


        public int RideId { get; set; }

        public List<Ride> Rides { get; set; }


        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }




    }
}
