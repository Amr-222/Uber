using Uber.DAL.Repo.Abstraction;
using Uber.DAL.DataBase;


namespace User.DAL.Repo.Impelementation
{
    public class UserRepo : IUserRepo
    {
        private readonly UberDBContext db;

        public UserRepo(UberDBContext db)
        {
            this.db = db;
        }
    }
}