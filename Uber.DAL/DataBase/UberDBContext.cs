using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uber.DAL.Entities;

namespace Uber.DAL.DataBase
{
    public class UberDBContext : IdentityDbContext<AppUser>
    {



        public UberDBContext(DbContextOptions<UberDBContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        //}

        public DbSet<Customer> Customers { get; set; }
    }
}
