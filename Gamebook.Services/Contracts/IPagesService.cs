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
        Task<int> Add(Page page);
        Task<int> Delete(Page page);
        Task<int> Update(Page page);
    }
}