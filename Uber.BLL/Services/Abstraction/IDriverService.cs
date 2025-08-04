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
        public (bool, string?) Create(CreateDriver driver);
        public (bool, string?) Delete(string id);
        public (bool, string?) Edit();
    }
}
