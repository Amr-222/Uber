using System.IO;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class Admin: ApplicationUser
    {
        public Admin(string name, Gender gender, DateTime DateOfBirth, string? ImagePath/*, string Street, string City*/)
        {
            this.Name = Name;
            this.Gender = Gender;        
            this.DateOfBirth = DateOfBirth;
            this.ImagePath = ImagePath;
            //this.Address = new Location { Street = Street, City = City };

        }
        public Admin()
        {
           
        }
    }
}
