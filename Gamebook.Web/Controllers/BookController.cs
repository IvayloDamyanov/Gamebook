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
        public ViewResult List(string searchTerm)
        {
            var books = this.booksService
                .FindAll(searchTerm)
                .Select(book => new BookListViewModel()
                {
                    CatalogueNumber = book.CatalogueNumber,
                    Title = book.Title
                })
                .ToList();

            //With Automapper (inject IMapper in constructor)
            //var books = this.booksService
            //    .Find(query)
            //    .Select(x => this.mapper.Map<PostViewModel>(x))
            //    .ToList();

            var viewModel = new ListViewModel()
            {
                Books = books
            };

            return View(viewModel);
        }

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
        
        [HttpGet]
        [Authorize]
        public ViewResult Edit(string query)
        {
            //var books = this.booksService
            //    .FindAll(query)
            //    .Select(book => new BookViewModel()
            //    {
            //        Id = book.Id,
            //        CatalogueNumber = book.CatalogueNumber,
            //        Title = book.Title,
            //        Resume = book.Resume,
            //        CreatedOn = book.CreatedOn.Value,
            //        ModifiedOn = book.ModifiedOn,
            //        isDeleted = book.isDeleted,
            //        DeletedOn = book.DeletedOn
            //    })
            //    .ToList();

            //var viewModel = new ListViewModel()
            //{
            //    Books = books
            //};

            //return View(viewModel);

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(BookViewModel model)
        {
            //this.booksService.Update();

            return this.RedirectToAction("Index");
        }

        
    }
}