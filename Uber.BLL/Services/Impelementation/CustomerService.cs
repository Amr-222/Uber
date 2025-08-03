using AutoMapper;
using Uber.BLL.Helper;
using Uber.BLL.ModelVM.Customer;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;
namespace Uber.BLL.Services.Impelementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepo customerRepo;

        public CustomerService(ICustomerRepo _userRepo, IMapper _mapper)
        {
            this.customerRepo = _userRepo;
            mapper = _mapper;
        }

        public (bool, string?) Create(CreateCustomer customer)
        {
            try
            {
                var cust = mapper.Map<Customer>(customer);
                cust.AddProfilePhoto(Upload.UploadFile("Files", customer.File));
                var result = customerRepo.Create(cust);
                return result;
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
                var result = customerRepo.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
