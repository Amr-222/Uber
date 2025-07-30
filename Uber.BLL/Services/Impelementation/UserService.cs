using AutoMapper;
using Uber.DAL.Repo.Abstraction;

namespace Uber.BLL.Services.Impelementation
{
    public class UserServices
    {
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;

        public UserServices(IUserRepo _userRepo, IMapper _mapper)
        {
            this.userRepo = _userRepo;
            mapper = _mapper;
        }
    }
}
