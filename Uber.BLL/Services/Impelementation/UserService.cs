using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Uber.BLL.Helper;
using Uber.BLL.ModelVM.User;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;
using Uber.DAL.Repo.Implementation;
namespace Uber.BLL.Services.Impelementation
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IUserRepo _userRepo, IMapper _mapper, UserManager<ApplicationUser> _userManager)
        {
            userRepo = _userRepo;
            mapper = _mapper;
            userManager = _userManager;
        }

        public async Task<(bool, string?)> CreateAsync(CreateUser user)
        {
            try
            {

                string errors = "";

                var user1 = mapper.Map<User>(user);

                user1.UserName = user.Email;

                //var result = userRepo.Create(user1);

                var res = await userManager.CreateAsync(user1, user.Password);


                if (res.Succeeded)
                {

                    return (true,null);
                }
                else
                {
                    foreach (var error in res.Errors)
                    {
                        errors += (($"{error.Code} - {error.Description}\n"));
                    }

                    return (false, errors);
                }
               

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
                //
                //Mappppppppppp
              //  userRepo.Edit();
                return (true,null);

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (string?, User?) GetByID(string id)
        {
            return userRepo.GetByID(id);
        }
        public List<User> GetAll()
        { 
            return userRepo.GetAll(); 
        }
    }
}
