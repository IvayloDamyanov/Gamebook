using System.Linq;
using Gamebook.Data.Model;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IPagesService : IService
    {
        IQueryable<Page> GetAll();
        IQueryable<Page> GetAllAndDeleted();
        Page Find(int bookCatalogueNumber, int searchedPageNum);
        int Add(Page page);
        int Delete(Page page);
        int Update(Page page);
    }
}