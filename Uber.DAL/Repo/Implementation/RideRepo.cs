using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;

namespace Uber.DAL.Repo.Implementation
{
    public class RideRepo : IRideRepo
    {

        private readonly UberDBContext db;
        public (bool, string?) Create(Ride ride)
        {
            try
            {
                db.Rides.Add(ride);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false,  ex.Message);
            }
        }

        public (bool, string?) Cancel(int id)
        {
            try
            {
                var ride = db.Rides.Where(a => a.Id == id).FirstOrDefault();
                if (ride == null)
                {
                    return (false, "Ride not found");
                }
                var result = ride.Cancel();
                db.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public List<Ride> GetAll()
        {
            
            return db.Rides.ToList();  

        }

        public (string, Ride?) GetByID(int id)
        {
            try
            {
                var ride = db.Rides.Where(a => a.Id == id).FirstOrDefault();
                if (ride == null)
                {
                    return ("Ride not found", null);
                }
                return (null, ride);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }
    }
}
