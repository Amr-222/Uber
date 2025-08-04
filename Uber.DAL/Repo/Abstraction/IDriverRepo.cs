using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.DAL.Entities;

namespace Uber.DAL.Repo.Abstraction
{
    public interface IDriverRepo
    {
        (bool, string?) Create(Driver driver);
        (string?, Driver?) GetByID(string id);
        List<Driver> GetAll();
        (bool, string?) Delete(string id);
        (bool, string?) Edit(Driver driver);
    }
}
