using Microsoft.AspNetCore.Identity;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public string Name { get; protected set; }

        public Gender Gender { get; protected set; }
        public int Age { get; protected set; }
        public DateTime DateOfBirth { get; protected set; }

        public bool IsDeleted { get; protected set; } = false;
        public DateTime CreatedAt { get; } = DateTime.Now;
        public string? ImagePath { get; protected set; }

        public Location Address { get; protected set; }

    }
}

