using Microsoft.AspNetCore.Identity;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public string Name { get; protected set; }

        public Gender Gender { get; protected set; }

        public int Age()
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
        public DateTime DateOfBirth { get; protected set; }

        public bool IsDeleted { get; protected set; } = false;
        public DateTime CreatedAt { get; } = DateTime.Now;
        public string? ImagePath { get; protected set; }

        public Location Address { get; protected set; }

    }
}

