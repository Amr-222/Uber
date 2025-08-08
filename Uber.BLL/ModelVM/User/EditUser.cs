using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uber.BLL.ModelVM.User
{
    public class EditUser
    {
        public string Name { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string? Imagepath { get; set; }
        //public Location Address { get; set; } //To be changed
    }
}
