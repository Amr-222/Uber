using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class User : ApplicationUser
    {

        // ID
        // Pass
        // Phone
        //public string Email { get; private set; }

        public DateTime? ModifiedAt { get; private set; }


        public int TotalRatingPoints { get; private set; } // the total points of all the people who rated this person
                                                           // (ex: this person was rated twice as 3 and 5 stars then the points will be 8)
        public int TotalRatings { get; private set; } // Total number of people who rated this person
        public double Rating() => TotalRatings != 0 ? (double)TotalRatingPoints / TotalRatings : 5; // the actual rating that we will display

        public User() { }

        public User( string name,Gender gender,DateTime DateOfBirth, string? ImagePath/*, string street,string city*/)
        {
            this.Name = name;
            this.Gender = gender;         
            this.DateOfBirth = DateOfBirth;
            this.ImagePath = ImagePath;
            //this.Address = new Location { Street = street, City = city };

        }
        //Navigation Property

        public int WalletId { get;  set; }

        public Wallet Wallet { get;  set; }



        public List <Ride> Rides { get; set; }






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





        public (bool, string?) Edit(string name, DateTime dateofbirth, string? imagepath/*, Location address*/)
        {
            try
            {
                this.Name = name;
                this.DateOfBirth = dateofbirth;
                this.ImagePath = imagepath;
                //this.Address = address;
                ModifiedAt = DateTime.Now;
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


    }
}
