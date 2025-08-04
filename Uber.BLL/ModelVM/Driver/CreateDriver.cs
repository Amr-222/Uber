using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uber.BLL.ModelVM.Driver
{
    public class CreateDriver
    {
        public IFormFile? File { get; set; }
    }
}
