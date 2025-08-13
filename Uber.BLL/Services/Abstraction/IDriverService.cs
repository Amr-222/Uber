using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.ModelVM.Driver;
using Uber.BLL.ModelVM.User;

namespace Uber.BLL.Services.Abstraction
{
    public interface IDriverService
    {
        public  Task<(bool, string?)> CreateAsync(CreateDriver driver);
        public (bool, string?) Delete(string id);
        public (bool, string?, List<string>?) GetNearestDriver(double lat, double lng);
        object SendRequest(GetDriver getDriver, string id);
        //public (bool, string?) Edit();
    }
}
