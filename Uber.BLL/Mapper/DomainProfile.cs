using AutoMapper;
using Uber.BLL.ModelVM.Customer;
using Uber.DAL.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uber.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Customer, CreateCustomer>().ReverseMap();
        }
    }
}
