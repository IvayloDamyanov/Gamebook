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
        Task<int> Add(User user);
        Task<int> Delete(User user);
        Task<int> Update(User user);
    }
}