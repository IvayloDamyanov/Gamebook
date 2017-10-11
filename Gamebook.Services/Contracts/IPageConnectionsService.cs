using System.Linq;
using Gamebook.Data.Model;

namespace Gamebook.Services.Contracts
{
    public interface IPageConnectionsService : IService
    {
        IQueryable<PageConnection> GetAll();
        IQueryable<PageConnection> getChildPages(int bookCatalogueNumber, int parentPageNumber);
    }
}