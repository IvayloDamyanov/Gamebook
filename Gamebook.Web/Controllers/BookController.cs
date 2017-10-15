using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using Gamebook.Web.Infrastructure;
using Gamebook.Web.Models.Book;
using Gamebook.Web.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamebook.Web.Controllers
{
    [RouteArea("Default")]
    public class BookController : Controller
    {
        private readonly IBooksService booksService;
        private readonly IPagesService pagesService;
        private readonly IPageConnectionsService pageConnectionsService;

        public BookController(
                            IBooksService booksService,
                            IPagesService pagesService,
                            IPageConnectionsService pageConnectionsService)
        {
            this.booksService = booksService;
            this.pagesService = pagesService;
            this.pageConnectionsService = pageConnectionsService;
        }

        // GET: \book - main page + search
        [HttpGet]
        [Authorize]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ViewResult Detailed(int id)
        {
            Book book = this.booksService.FindSingle(id);

            BookDetailedViewModel viewModel = new BookDetailedViewModel()
            {
                CatalogueNumber = book.CatalogueNumber,
                Title = book.Title,
                Resume = book.Resume
            };

            return View(viewModel);
        }

        // GET: \book\list - search results
        [HttpGet]
        [Authorize]
        public ViewResult List(string searchTerm, int resultsPerPage = 5, int page = 1)
        {
            var books = this.booksService
                .FindAll(searchTerm)
                .Where(book => book.isDeleted != true)
                .Select(book => new BookListViewModel()
                {
                    CatalogueNumber = book.CatalogueNumber,
                    Title = book.Title
                })
                .OrderBy(x => x.CatalogueNumber)
                .ToList();

            page = page > 0 ? page : 1;

            var booksRange = booksService.Pagination(books.Count, resultsPerPage, page);
            var outputBooks = books.GetRange(booksRange.Item1, booksRange.Item2);

            int[] pagesNav = booksService.PagesNav(books.Count, resultsPerPage, page);

            //With Automapper (inject IMapper in constructor)
            //var books = this.booksService
            //    .Find(query)
            //    .Select(x => this.mapper.Map<PostViewModel>(x))
            //    .ToList();

            var viewModel = new ListViewModel()
            {
                Pages = pagesNav,
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                SearchedTerm = searchTerm,
                Books = outputBooks
            };

            return View(viewModel);
        }

        //private int[] PagesNav(int booksCount, int resultsPerPage, int page)
        //{
        //    List<int> pages = new List<int>();
        //    if (booksCount == 0)
        //    {
        //        return pages.ToArray();
        //    }

        //    int pagesCount = booksCount % resultsPerPage == 0 ? booksCount / resultsPerPage : (booksCount / resultsPerPage) + 1;
        //    int listSize = 5;
        //    int pageNum = page + (listSize / 2) < pagesCount ? page + (listSize / 2) : pagesCount;
        //    while (pagesCount > 0 && listSize > 0 && pageNum > 0)
        //    {
        //        pages.Add(pageNum);
        //        pageNum--;
        //        pagesCount--;
        //        listSize--;
        //    }

        //    pages.Reverse();

        //    return pages.ToArray();
        //}

        //private List<BookListViewModel> Pagination(List<BookListViewModel> books, int resultsPerPage, int page)
        //{
        //    var outputBooks = new List<BookListViewModel>();
        //    int booksCount = books.Count;
        //    int startIndex = 0;
        //    int resultsCount = resultsPerPage;

        //    if (booksCount == 0)
        //    {
        //        return books;
        //    }

        //    if (books.Count >= resultsPerPage * (page - 1))
        //    {
        //        startIndex = resultsPerPage * (page - 1);
        //    }

        //    if (booksCount < resultsPerPage * page && booksCount >= resultsPerPage * (page - 1))
        //    {
        //        resultsCount = (booksCount % resultsPerPage);
        //    }

        //    if (booksCount < resultsPerPage * (page - 1))
        //    {
        //        resultsCount = 0;
        //    }

        //    if (resultsCount > 0)
        //    {
        //        outputBooks = books.GetRange(startIndex, resultsCount);
        //    }

        //    return outputBooks;
        //}

        [HttpGet]
        public ViewResult Read(int book, int page)
        {
            Page targetPage = this.pagesService
                .Find(book, page);
            var childPages = this.pageConnectionsService
                .getChildPages(book, page)
                .Select(pageConnection => new PageConnectionViewModel()
                {
                    Text = pageConnection.Text,
                    ChildPageNumber = pageConnection.ChildPageNumber
                })
                .ToList();

            PageDetailedViewModel viewModel = new PageDetailedViewModel()
            {
                BookCatalogueNumber = targetPage.Book.CatalogueNumber,
                Number = targetPage.Number,
                Text = targetPage.Text,
                ChildPages = childPages
            };

            return View(viewModel);
        }
    }
}