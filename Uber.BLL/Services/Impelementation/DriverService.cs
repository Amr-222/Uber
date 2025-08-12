using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.Helper;
using Uber.BLL.ModelVM.Driver;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;

namespace Uber.BLL.Services.Impelementation
{
    public class DriverService : IDriverService
    {
        private readonly IMapper mapper;
        private readonly IDriverRepo driverRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public DriverService(IDriverRepo _driverRepo, IMapper _mapper, UserManager<ApplicationUser> userManager)
        {
            this.driverRepo = _driverRepo;
            mapper = _mapper;
            this.userManager = userManager;
        }

        public async Task<(bool, string?)> CreateAsync(CreateDriver driver)
        {
            try
            {
                string errors = "";

                var driv = mapper.Map<Driver>(driver);

                driv.UserName = driver.Email;

                driv.AddProfilePhoto(Upload.UploadFile("Files", driver.File));


                var result = driverRepo.CreateVehicle(driv.Vehicle);

                if (!result.Item1)
                    return result;


                var res = await userManager.CreateAsync(driv, driver.Password);

                if (res.Succeeded)
                {

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
                var result = driverRepo.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }









    }
}
