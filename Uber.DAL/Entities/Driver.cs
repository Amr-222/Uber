using System.Reflection;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class Driver : ApplicationUser
    {
        //License  **

        public DateTime? ModifiedAt { get; private set; }


        public double Balance { get; private set; } = 0;
        public string? ImagePath { get; protected set; }


        public int TotalRatingPoints { get; private set; }

        public int TotalRatings { get; private set; }
        public double Rating() => TotalRatings != 0 ? (double)TotalRatingPoints / TotalRatings : 5;

        public Driver()
        {
            //Wallet =new Wallet();
        }

        public Driver(string name, Gender gender, DateTime DateOfBirth, string? ImagePath/*, string street, string city*/)
        {
            this.Name = name;
            this.Gender = gender;
            this.DateOfBirth = DateOfBirth;
            this.ImagePath = ImagePath;
            //Wallet = new Wallet();
            //this.Address = new Location { Street = street, City = city };

        }

        //Navigation Property


        //public int WalletId { get; set; }

        //public Wallet Wallet { get; set; }

        public double CurrentLng { get; private set; }
        public double CurrentLat { get; private set; }
        public bool IsActive { get; private set; } = false;
        public List<Ride> Rides { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public (bool, string?) Delete()
        {
            try
            {
                IsDeleted = true;
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public void AddProfilePhoto(string path)
        {
            if (IsDeleted) return;
            ImagePath = path;
        }
        public (bool, string?) Edit(string name, DateTime dateofbirth,string ImagePath, string Email, string PhoneNumber,bool isdeleted)
        {



            try
            {
                this.Name = name;
                this.DateOfBirth = dateofbirth;
                this.Email = Email;
                this.PhoneNumber = PhoneNumber;
                this.ImagePath = ImagePath;
                this.IsDeleted = isdeleted; 


                ModifiedAt = DateTime.Now;
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) MakeActive()
        {
            try
            {
                IsActive = true;
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public (bool, string?) MakeInactive()
        {

            try
            {
                IsActive = false; return (true, null);
            }

            catch (Exception ex) 
            { 
                return (false, ex.Message); 
            }

        }


    }
}
