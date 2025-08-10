using System.IO;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class Admin: ApplicationUser
    {
        public Admin(string name, Gender gender, DateTime DateOfBirth) { 
            this.Name = Name;
            this.Gender = Gender;        
            this.DateOfBirth = DateOfBirth;
           
     

        }
        public Admin()
        {
           
        }
    }
}
