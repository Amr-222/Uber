using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uber.BLL.ModelVM.Driver
{
    public class EditDriver
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Imagepath { get; set; }
        public string? ImagePath { get; internal set; }
    }
}
