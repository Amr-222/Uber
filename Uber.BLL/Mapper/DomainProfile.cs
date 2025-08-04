using AutoMapper;
using Uber.BLL.ModelVM.User;
using Uber.BLL.ModelVM.Driver;
using Uber.DAL.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uber.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<User, CreateUser>().ReverseMap();
            CreateMap<Driver, CreateDriver>().ReverseMap();
        }
    }
}
