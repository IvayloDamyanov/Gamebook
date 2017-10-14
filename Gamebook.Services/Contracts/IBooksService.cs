using System.Linq;
using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IBooksService : IService
    {
        IQueryable<Book> GetAll();
        IQueryable<Book> GetAllAndDeleted();
        IQueryable<Book> FindAll(string query);
        Book FindSingle(int catalogueNumber);
        Task<int> Add(Book book);
        Task<int> Delete(Book book);
        Task<int> Update(Book book);
    }
}