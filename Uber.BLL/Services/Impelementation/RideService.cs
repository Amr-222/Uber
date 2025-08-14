using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.Helper;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Enums;
using Uber.DAL.Repo.Abstraction;
using Uber.DAL.Repo.Impelementation;

namespace Uber.BLL.Services.Impelementation
{
     public class RideService : IRideService
    {
        private readonly IRideRepo rideRepo;
        public RideService(IRideRepo rideRepo)
        {
            this.rideRepo = rideRepo;   
        }
        public (bool, string?, Ride?) CreatePendingRide(
        string userId, string driverId,
        double startLat, double startLng, 
        double endLat, double endLng/*,
        double Distance, double Duration, double Price*/)
        {
            try
            {
                var ride = new Ride
                {
                    UserId = userId,
                    DriverId = driverId,
                    StartLat = startLat,
                    StartLng = startLng,
                    EndLat = endLat,
                    EndLng = endLng,
                    Status = RideStatus.Pending,
                    Duration = null,  // Will be calculated later
                    Distance = null,  // Will be calculated later
                    Price = null      // Will be calculated later
                };
                var (ok, err) = rideRepo.Create(ride);
                return (ok, err, ok ? ride : null);
            }
            catch (Exception ex) { return (false, ex.Message, null); }
        }

        public (bool, string?) MarkAccepted(int id) => rideRepo.UpdateStatus(id, RideStatus.Accepted);
        public (bool, string?) MarkRejected(int id) => rideRepo.UpdateStatus(id, RideStatus.Rejected);


        public (bool, string?) Create(Ride ride)
        {


            try
            {
                var result = rideRepo.Create(ride);
                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool, string?) Cancel(int id)
        {
            try
            {
                var result = rideRepo.Cancel(id);
                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (string?, Ride?) GetByID(int id) 
        {
            return rideRepo.GetByID(id);
        }
        public List<Ride> GetAll()
        {
            return rideRepo.GetAll();
        }

        public (bool, string?) AssignNewDriver(int rideId, string newDriverId)
        {
            try
            {
                var (ok, err) = rideRepo.AssignNewDriver(rideId, newDriverId);
                return (ok, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) MarkInProgress(int id)
        {
            try
            {
                var (ok, err) = rideRepo.UpdateStatus(id, RideStatus.InProgress);
                return (ok, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) MarkCompleted(int id)
        {
            try
            {
                var (ok, err) = rideRepo.UpdateStatus(id, RideStatus.Completed);
                return (ok, err);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
