using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;

namespace Uber.BLL.Services.Abstraction
{
    public interface IRideService
    {
        (bool, string?) Create(Ride ride);
        (bool, string?) Cancel(int id);
        (string?, Ride?) GetByID(int id);
        List<Ride> GetAll();

        (bool, string?, Ride?) CreatePendingRide(string userId, string driverId,
            double startLat, double startLng, double endLat, double endLng);

        (bool, string?) MarkAccepted(int id);
        (bool, string?) MarkRejected(int id);
    }
}
