using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Gamebook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Services
{
    public class UsersService : IUsersService
    {
        private readonly IEfRepository<User> usersRepo;
        private readonly ISaveContext context;

        public UsersService(IEfRepository<User> usersRepo, ISaveContext context)
        {
            this.usersRepo = usersRepo;
            this.context = context;
        }

        public IQueryable<User> GetAll()
        {
            return this.usersRepo.All;
        }

        public User FindSingle(string userName)
        {
            return this.usersRepo
                .All
                .Where(user => user.UserName == userName)
                .First();
        }

        public Task<int> Add(User user)
        {
            this.usersRepo.Add(user);
            return this.context.CommitAsync();
        }

        public Task<int> Delete(User user)
        {
            this.usersRepo.Delete(user);
            return this.context.CommitAsync();
        }

        public Task<int> Update(User user)
        {
            this.usersRepo.Update(user);
            return this.context.CommitAsync();
        }
    }
}
