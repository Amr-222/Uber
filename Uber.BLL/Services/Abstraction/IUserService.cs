using Uber.BLL.ModelVM.User;

namespace Uber.BLL.Services.Abstraction
{
    public interface IUserService
    {
        public (bool, string?) Create(CreateUser user);
        (bool, string?) Delete(string id);
        //(bool, string?) Update(EmployeeDTO employee);
        //(string?, EmployeeDTO?) GetByID(int id);
        //List<Employee> GetAll();
    }
}
