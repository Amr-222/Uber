using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.Helper;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;
using Uber.DAL.Repo.Impelementation;

namespace Uber.BLL.Services.Impelementation
{
     public class RideService
    {
        private readonly IRideRepo rideRepo;

        public RideService(IRideRepo rideRepo)
        {
            this.rideRepo = rideRepo;   
        }
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
    }
}
