using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using Gamebook.Web.Areas.Administration.Models;
using Gamebook.Web.Infrastructure;
using Gamebook.Web.Models.Book;
using Gamebook.Web.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gamebook.Web.Areas.Administration.Controllers
{
    [RouteArea("Administration")]
    [Authorize(Roles = "Admin")]
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
        public ViewResult Edit(int id)
        {
            Book book = this.booksService.FindSingle(id);

            BookFullViewModel viewModel = new BookFullViewModel()
            {
                Id = book.Id,
                CatalogueNumber = book.CatalogueNumber,
                Title = book.Title,
                Resume = book.Resume,
                isDeleted = book.isDeleted,
                DeletedOn = book.DeletedOn,
                CreatedOn = book.CreatedOn,
                ModifiedOn = book.ModifiedOn,
                AuthorUsername = book.Author.UserName,
                AuthorId = book.Author.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(BookFullViewModel model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            Book book = new Book()
            {
                Id = model.Id,
                Title = model.Title,
                CatalogueNumber = model.CatalogueNumber,
                Resume = model.Resume,
                CreatedOn = model.CreatedOn,
                ModifiedOn = DateTime.Now
            };

            if (model.isDeleted && model.DeletedOn == null)
            {
                book.isDeleted = true;
                book.DeletedOn = DateTime.Now;
            }
            if (!model.isDeleted && model.DeletedOn != null)
            {
                book.isDeleted = false;
                book.DeletedOn = null;
            }

            var result = this.booksService.Update(book);
            return this.RedirectToAction("Result", "Home", new { result = result });
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
        [Authorize]
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