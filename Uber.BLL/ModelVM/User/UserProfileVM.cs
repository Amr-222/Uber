using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;

namespace Uber.BLL.ModelVM.User
{
    public class UserProfileVM
    {
        public string Name { get; set; }
        public List<Ride> Rides { get; set; }
        public string PaymentMethod { get; set; }
    }
}
