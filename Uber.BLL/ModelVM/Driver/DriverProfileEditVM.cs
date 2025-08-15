using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;
using Uber.DAL.Enums;

namespace Uber.BLL.ModelVM.Driver
{
     public class DriverProfileEditVM
    {
        public DriverProfileVM Profile { get; set; }   
        public EditDriver Edit { get; set; }
        public double Balance => Profile?.Balance ?? 0;
        public string Name => Profile?.Name ?? "";
        public List<Uber.DAL.Entities.Ride> Rides => Profile?.Rides ?? new List<Uber.DAL.Entities.Ride>();
        public string? ImagePath => Profile?.ImagePath;
        public IFormFile? File => Profile?.file;
        public string Email => Edit?.Email ?? "";
        public string Phone => Edit?.PhoneNumber ?? "";
        public DateTime Dateofbirth => Edit?.DateOfBirth ?? DateTime.Now;
        public Gender Gender => Edit?.Gender ?? Uber.DAL.Enums.Gender.Male;
        public Vehicle? Vehicle => Edit?.Vehicle;
        
    }
}