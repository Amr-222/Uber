using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uber.BLL.ModelVM.User
{
    public class CreateUser
    {
        public IFormFile? File { get; set; }
    }
}
