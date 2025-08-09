using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uber.BLL.Helper;
using Uber.BLL.ModelVM.Driver;
using Uber.DAL.Entities;
using Uber.DAL.Repo.Abstraction;
using Uber.BLL.Services.Abstraction;

namespace Uber.BLL.Services.Impelementation
{
    public class DriverService : IDriverService
    {
        private readonly IMapper mapper;
        private readonly IDriverRepo driverRepo;

        public DriverService(IDriverRepo _driverRepo, IMapper _mapper)
        {
            this.driverRepo = _driverRepo;
            mapper = _mapper;
        }

        public (bool, string?) Create(CreateDriver driver)
        {
            try
            {
                var driv = mapper.Map<Driver>(driver);
                driv.AddProfilePhoto(Upload.UploadFile("Files", driver.File));
                var result = driverRepo.Create(driv);
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
