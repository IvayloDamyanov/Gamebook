using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Gamebook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamebook.Services
{
    public class PagesService : IPagesService
    {
        private readonly IEfRepository<Page> pagesRepo;
        private readonly IBooksService booksService;
        private readonly ISaveContext context;

        public PagesService(IEfRepository<Page> pagesRepo, IBooksService booksService, ISaveContext context)
        {
            this.pagesRepo = pagesRepo;
            this.booksService = booksService;
            this.context = context;
        }

        public IQueryable<Page> GetAll()
        {
            return this.pagesRepo.All;
        }

        public Page Find(int bookCatalogueNumber, int searchedPageNum)
        {
            Book searchedBook = this.booksService.FindSingle(bookCatalogueNumber);

            Page searchedPage = this.pagesRepo
                .All
                .Where(page =>
                    page.Book.Id == searchedBook.Id
                    && page.Number == searchedPageNum)
                .First();
            if(searchedPage == null)
            {
                return new Page();
            }
            else
            {
                return searchedPage;
            }
        }

        public void Update(Page page)
        {
            this.pagesRepo.Update(page);
            this.context.Commit();
        }
    }
}
