using Microsoft.EntityFrameworkCore;
using Uber.DAL.Entities;
namespace Uber.DAL.DataBase
{
    public class UberDBContext : DbContext
    {



        public UberDBContext(DbContextOptions<UberDBContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
