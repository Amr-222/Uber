using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;
using Uber.DAL.Enums;
using Uber.DAL.Repo.Abstraction;

namespace Uber.DAL.Repo.Implementation
{
    public class RideRepo : IRideRepo
    {

        private readonly UberDBContext db;
        public RideRepo(UberDBContext db)
        {
            this.db = db;
        }

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
        public (bool, string?) UpdateStatus(int id, RideStatus status)
        {
            var r = db.Rides.Find(id);
            if (r == null) return (false, "Not found");
            r.Status = status;
            db.SaveChanges();
            return (true, null);
        }

        public (bool, string?) AssignNewDriver(int rideId, string newDriverId)
        {
            try
            {
                var ride = db.Rides.Find(rideId);
                if (ride == null) return (false, "Ride not found");
                
                ride.DriverId = newDriverId;
                ride.Status = RideStatus.Pending; // Reset status to pending for new driver
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
