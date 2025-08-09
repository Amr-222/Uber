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
        public (bool, string?) Create(Ride ride);// TODO Change this
        public (bool, string?) Cancel(int id);// TODO Change this
        public (string?, Ride?) GetByID(int id); // TODO Change this
        public List<Ride> GetAll();// TODO Change this
    }
}
