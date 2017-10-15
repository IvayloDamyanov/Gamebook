using System.Linq;
using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IUsersService : IService
    {
        User FindSingle(string Id);
        IQueryable<User> GetAll();
        IQueryable<User> GetAllAndDeleted();
        int Add(User user);
        int Delete(User user);
        int Update(User user);
    }
}