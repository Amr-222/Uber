using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;
using Uber.DAL.Enums;

namespace Uber.DAL.Repo.Abstraction
{
    public interface IRideRepo
    {
        public (bool, string?) Create(Ride ride);
        public (bool, string?) Cancel(int id);
        public (string, Ride?) GetByID(int id);
        public List<Ride> GetAll();

        public (bool, string?) UpdateStatus(int id, RideStatus status);
        public (bool, string?) AssignNewDriver(int rideId, string newDriverId);
    }
}
