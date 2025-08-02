using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Uber.DAL.DataBase
{
    public class UberDBContext : IdentityDbContext<User>
    {



        public UberDBContext(DbContextOptions<UberDBContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        //}

     //   public DbSet<Employee> Employees { get; set; }
    }
}
