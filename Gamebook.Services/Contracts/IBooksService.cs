using System.Linq;
using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IBooksService : IService
    {
        IQueryable<Book> GetAll();
        IQueryable<Book> FindAll(string query);
        Book FindSingle(int id);
        int Update(Book book);
    }
}