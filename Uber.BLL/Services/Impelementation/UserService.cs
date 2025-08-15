using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IUserRepo _userRepo, IMapper _mapper, UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor)
        {
            userRepo = _userRepo;
            mapper = _mapper;
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
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
                    await userManager.AddToRoleAsync(user1, "User");
                    return (true, null);
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
                var result = userRepo.GetByID(user.Id);
                var existingUser = result.Item2;
                if (existingUser == null)
                    return (false, "User not found");

                //existingUser.Edit(user.Name, user.DateOfBirth,user.Email,user.PhoneNumber);

                userRepo.Edit(existingUser);

                return (true, null);
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
        public List<EditUser> GetAll()
        {
            var result = userRepo.GetAll();
            var list = new List<EditUser>();
            foreach (var user in result)
            {
                list.Add(mapper.Map<EditUser>(user));
            }
            return list;
        }

        public async Task<(bool, string?, UserProfileVM?)> GetProfileInfo()
        {
            try
            {
                var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
                if (user == null)
                {
                    return (false, "User not found or not logged in.", null);
                }

                var userProfile = mapper.Map<UserProfileVM>(user);

                return (true, null, userProfile);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        

        public (string?, EditUser?) GetByIDToEdit(string id)
        {
            try
            {
                var result = userRepo.GetByID(id);
                if (result.Item1 != null) return (result.Item1, null);
                var user = mapper.Map<EditUser>(result.Item2);
                return (null, user);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }


    }
}
