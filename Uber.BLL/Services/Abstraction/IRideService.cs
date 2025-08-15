using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.ModelVM.Ride;
using Uber.DAL.Entities;

namespace Uber.BLL.Services.Abstraction
{
    public interface IRideService
    {
        (bool, string?) Create(Ride ride);
        (bool, string?) Cancel(int id);
        (string?, Ride?) GetByID(int id);
        public List<RideVM> GetAll();
        (bool, string?, Ride?) CreatePendingRide(
        string userId, string driverId, double startLat, double startLng,
        double endLat, double endLng, double Distance, double Duration, double Price);

        (bool, string?) MarkAccepted(int id);
        (bool, string?) MarkRejected(int id);
        (bool, string?) AssignNewDriver(int rideId, string newDriverId);
        (bool, string?) MarkDriverWaiting(int id);  // New method for when driver arrives at pickup
        (bool, string?) MarkInProgress(int id);
        (bool, string?) MarkCompleted(int id);
        (bool, string?) AddUserRating(int rideId, int rating);  // New method for rating user
        (bool, string?) AddDriverRating(int rideId, int rating);  // New method for rating driver
        string? GetRideById(int id);
    }
}
