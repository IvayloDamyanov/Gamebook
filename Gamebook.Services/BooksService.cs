using Gamebook.Data.Model;
using Gamebook.Data.Repositories.Contracts;
using Gamebook.Data.SaveContext.Contracts;
using Gamebook.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamebook.Services
{
    public class BooksService : IBooksService
    {
        private readonly IEfRepository<Book> booksRepo;
        private readonly ISaveContext context;

        public BooksService(IEfRepository<Book> booksRepo, ISaveContext context)
        {
            this.booksRepo = booksRepo;
            this.context = context;
        }

        public IQueryable<Book> GetAll()
        {
            return this.booksRepo.All;
        }

        public IQueryable<Book> GetAllAndDeleted()
        {
            return this.booksRepo.AllAndDeleted;
        }

        public IQueryable<Book> FindAll(string searchTerm)
        {
            if (searchTerm == string.Empty || searchTerm == null)
            {
                return this.GetAll();
            }
            else
            {
                int bookCatNum;
                try
                {
                    bookCatNum = int.Parse(searchTerm);
                }
                catch
                {
                    bookCatNum = -1;
                }
                return this.booksRepo
                    .All
                    .Where(book =>
                        book.Title.Contains(searchTerm)
                        || book.CatalogueNumber == bookCatNum);
            }
        }

        public Book FindSingle(int catalogueNumber)
        {
            return this.booksRepo
                .AllAndDeleted
                .Where(book => book.CatalogueNumber == catalogueNumber)
                .FirstOrDefault();
        }

        public int Add(Book book)
        {
            this.booksRepo.Add(book);
            return this.context.Commit();
        }

        public int Delete(Book book)
        {
            this.booksRepo.Delete(book);
            return this.context.Commit();
        }

        public int Update(Book book)
        {
            this.booksRepo.Update(book);
            return this.context.Commit();
        }

        public int[] PagesNav(int booksCount, int resultsPerPage, int page)
        {
            List<int> pages = new List<int>();
            if (booksCount == 0)
            {
                return pages.ToArray();
            }

            int pagesCount = booksCount % resultsPerPage == 0 ? booksCount / resultsPerPage : (booksCount / resultsPerPage) + 1;
            int listSize = 5;
            int pageNum = page + (listSize / 2) < pagesCount ? page + (listSize / 2) : pagesCount;
            while (pagesCount > 0 && listSize > 0 && pageNum > 0)
            {
                pages.Add(pageNum);
                pageNum--;
                pagesCount--;
                listSize--;
            }

            pages.Reverse();

            return pages.ToArray();
        }

        public Tuple<int, int> Pagination(int booksCount, int resultsPerPage, int page)
        {
            Tuple<int, int> output = new Tuple<int, int>(0, 0);
            int startIndex = 0;
            int resultsCount = resultsPerPage;

            if (booksCount == 0)
            {
                return output;
            }

            if (booksCount >= resultsPerPage * (page - 1))
            {
                startIndex = resultsPerPage * (page - 1);
            }

            if (booksCount < resultsPerPage * page && booksCount >= resultsPerPage * (page - 1))
            {
                resultsCount = (booksCount % resultsPerPage);
            }

            if (booksCount < resultsPerPage * (page - 1))
            {
                resultsCount = 0;
            }

            if (resultsCount > 0)
            {
                output = new Tuple<int, int>(startIndex, resultsCount);
            }

            return output;
        }

    }
}
