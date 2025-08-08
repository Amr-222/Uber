using AutoMapper;
using Uber.BLL.Helper;
using Uber.BLL.ModelVM.User;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;
namespace Uber.BLL.Services.Impelementation
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;

        public UserService(IUserRepo _userRepo, IMapper _mapper)
        {
            this.userRepo = _userRepo;
            mapper = _mapper;
        }

        public (bool, string?) Create(CreateUser user)
        {
            try
            {
                var cust = mapper.Map<DAL.Entities.User>(user);
                cust.AddProfilePhoto(Upload.UploadFile("Files", user.File));
                var result = userRepo.Create(cust);
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
                var result = userRepo.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool, string?) Edit(EditUser user)
        {
            try
            {

            }
            catch (Exeption ex)
            {

            }
        }
    }
}
