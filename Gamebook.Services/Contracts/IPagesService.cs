using System.Linq;
using Gamebook.Data.Model;

namespace Gamebook.Services.Contracts
{
    public interface IPagesService : IService
    {
        IQueryable<Page> GetAll();
        Page Find(int bookCatalogueNumber, int searchedPageNum);
        void Update(Page page);
    }
}