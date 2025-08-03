using Uber.BLL.ModelVM.Customer;

namespace Uber.BLL.Services.Abstraction
{
    public interface ICustomerService
    {
        public (bool, string?) Create(CreateCustomer customer);
        (bool, string?) Delete(string id);
        //(bool, string?) Update(EmployeeDTO employee);
        //(string?, EmployeeDTO?) GetByID(int id);
        //List<Employee> GetAll();
    }
}
