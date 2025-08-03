namespace Uber.DAL.Entities
{
    public class Driver : AppUser
    {
        public int TotalRatingPoints { get; private set; } // the total points of all the people who rated this person
                                                           // (ex: this person was rated twice as 3 and 5 stars then the points will be 8)
        public double TotalRatings { get; private set; } // Total number of people who rated this person
        public double Rating => TotalRatingPoints / TotalRatings; // the actual rating that we will display 
        public DateTime DateOfBirth { get; private set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime? ModifiedAt { get; private set; }
        public string? ProfilePhotoPath { get; private set; }
        public Wallet Wallet { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        // Maybe add current location data member maybe as a hangfire
        // or every time the driver wants to accept a new ride
        public Driver() { }
    }
}
