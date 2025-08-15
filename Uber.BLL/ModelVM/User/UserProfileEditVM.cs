using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;
using Uber.DAL.Enums;

namespace Uber.BLL.ModelVM.User
{
     public class UserProfileEditVM
    {
        public UserProfileVM Profile { get; set; }   
        public EditUser Edit { get; set; }
        public string Name => Profile?.Name ?? "";
        public List<Uber.DAL.Entities.Ride> Rides => Profile?.Rides ?? new List<Uber.DAL.Entities.Ride>();
        public string Email => Edit?.Email ?? "";
        public string Phone => Edit?.PhoneNumber ?? "";
        public DateTime Dateofbirth => Edit?.DateOfBirth ?? DateTime.Now;
        
    }
}