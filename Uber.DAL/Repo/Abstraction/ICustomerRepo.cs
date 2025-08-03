using Uber.DAL.Entities;
namespace Uber.DAL.Repo.Abstraction
{
    public interface ICustomerRepo
    {
        (bool, string?) Create(Customer customer);
        (string?, Customer?) GetByID(string id);
        List<Customer> GetAll();
        (bool, string?) Delete(string id);
        (bool, string?) Update(Customer employee);
    }
}