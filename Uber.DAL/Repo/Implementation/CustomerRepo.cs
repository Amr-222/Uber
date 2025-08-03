using Uber.DAL.Repo.Abstraction;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;

namespace User.DAL.Repo.Impelementation
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly UberDBContext db;

        public CustomerRepo(UberDBContext db)
        {
            this.db = db;
        }

        public (bool, string?) Create(Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(string id)
        {
            try
            {
                var customer = db.Customers.Where(a => a.Id == id).FirstOrDefault();
                if (customer == null)
                {
                    return (false, "Customer not found");
                }
                customer.Delete();
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public List<Customer> GetAll()
        {
            return db.Customers.ToList();
        }

        public (string?, Customer?) GetByID(string id)
        {
            try
            {
                var customer = db.Customers.Where(a => a.Id == id).FirstOrDefault();
                if (customer == null)
                {
                    return ("Customer not found", null);
                }
                return (null, customer);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }

        public (bool, string?) Update(Customer customer)
        {
            try
            {
                var c = db.Customers.Where(a => a.Id == customer.Id).FirstOrDefault();
                if (c == null)
                {
                    return (false, "Employee not found");
                }
                //c.Update(); TODO
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}