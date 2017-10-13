using System.Linq;
using Gamebook.Data.Model;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IPageConnectionsService : IService
    {
        IQueryable<PageConnection> GetAll();
        IQueryable<PageConnection> getChildPages(int bookCatalogueNumber, int parentPageNumber);
        Task<int> Add(PageConnection pageConnection);
        Task<int> Delete(PageConnection pageConnection);
        Task<int> Update(PageConnection pageConnection);
    }
}