using Uber.BLL.ModelVM.User;

namespace Uber.BLL.Services.Abstraction
{
    public interface IUserService
    {
        public (bool, string?) Create(CreateUser user);
        public (bool, string?) Delete(string id);
        public (bool, string?) Edit(EditUser user);
        //(string?, EmployeeDTO?) GetByID(int id);
        //List<Employee> GetAll();
    }
}
