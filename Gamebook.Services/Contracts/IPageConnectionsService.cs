using System.Linq;
using Gamebook.Data.Model;
using System.Threading.Tasks;

namespace Gamebook.Services.Contracts
{
    public interface IPageConnectionsService : IService
    {
        IQueryable<PageConnection> GetAll();
        IQueryable<PageConnection> GetAllAndDeleted();
        IQueryable<PageConnection> GetChildPages(int bookCatalogueNumber, int parentPageNumber);
        int Add(PageConnection pageConnection);
        int Delete(PageConnection pageConnection);
        int Update(PageConnection pageConnection);
    }
}