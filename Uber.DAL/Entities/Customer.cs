namespace Uber.DAL.Entities
{
    public class Customer : AppUser
    {

        public int TotalRatingPoints { get; private set; } // the total points of all the people who rated this person
                                                           // (ex: this person was rated twice as 3 and 5 stars then the points will be 8)
        public double TotalRatings { get; private set; } // Total number of people who rated this person
        public double Rating => TotalRatings != 0 ? TotalRatingPoints / TotalRatings : 5; // the actual rating that we will display 
        public DateTime DateOfBirth { get; private set; }
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                // Adjust age if birthday hasn't occurred yet this year
                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime? ModifiedAt { get; private set; }
        public string? ProfilePhotoPath { get; private set; }
        public Wallet Wallet { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        public Customer(DateTime DoB, string? photo) 
        {
            TotalRatingPoints = 0;
            TotalRatings = 0;
            ProfilePhotoPath = photo;
            DateOfBirth = DoB;
        }

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
            ProfilePhotoPath = path;
        }
    }
}
