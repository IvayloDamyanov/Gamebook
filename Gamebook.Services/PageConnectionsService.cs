using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Gamebook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Services
{
    public class PageConnectionsService : IPageConnectionsService
    {
        private readonly IEfRepository<PageConnection> pageConnectionsRepo;
        private readonly IBooksService booksService;
        private readonly ISaveContext context;

        public PageConnectionsService(IEfRepository<PageConnection> pageConnectionsRepo, IBooksService booksService, ISaveContext context)
        {
            this.pageConnectionsRepo = pageConnectionsRepo;
            this.booksService = booksService;
            this.context = context;
        }

        public IQueryable<PageConnection> GetAll()
        {
            return this.pageConnectionsRepo.All;
        }

        public IQueryable<PageConnection> getChildPages(int bookCatalogueNumber, int parentPageNumber)
        {
            Book searchedBook = booksService.FindSingle(bookCatalogueNumber);

            return this.pageConnectionsRepo
                .All
                .Where(pageConnection =>
                    pageConnection.Book.Id == searchedBook.Id
                    && pageConnection.ParentPageNumber == parentPageNumber);
        }
    }
}
