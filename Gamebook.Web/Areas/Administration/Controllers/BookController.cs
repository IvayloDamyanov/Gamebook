using DataTables.Mvc;
using Gamebook.Data.Model;
using Gamebook.Services.Contracts;
using Gamebook.Web.Areas.Administration.Models;
//using Gamebook.Web.Infrastructure;
using Gamebook.Web.Models.Book;
using Gamebook.Web.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace Gamebook.Web.Areas.Administration.Controllers
{
    [RouteArea("Administration")]
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly IBooksService booksService;
        private readonly IPagesService pagesService;
        private readonly IPageConnectionsService pageConnectionsService;
        private readonly IUsersService usersService;

        public BookController(
                            IBooksService booksService,
                            IPagesService pagesService,
                            IPageConnectionsService pageConnectionsService,
                            IUsersService usersService)
        {
            this.booksService = booksService;
            this.pagesService = pagesService;
            this.pageConnectionsService = pageConnectionsService;
            this.usersService = usersService;
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
            Book book = this.booksService.FindSingle(model.CatalogueNumber);
            book.Title = model.Title;
            book.CatalogueNumber = model.CatalogueNumber;
            book.Resume = model.Resume;
            book.ModifiedOn = DateTime.Now;

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

            return this.RedirectToAction("List", "Book", new { result = result });
        }

        [Authorize]
        public ViewResult List(int result = 0)
        {
            var model = new ResultViewModel()
            {
                Result = result
            };
            return View(model);
        }

        [Authorize]
        public ActionResult BookTable([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var query = booksService
                .GetAllAndDeleted()
                .Select(book => new BookFullViewModel()
                {
                    Id = book.Id,
                    CatalogueNumber = book.CatalogueNumber,
                    Title = book.Title,
                    Resume = book.Resume,
                    isDeleted = book.isDeleted,
                    DeletedOn = book.DeletedOn,
                    CreatedOn = book.CreatedOn,
                    ModifiedOn = book.ModifiedOn,
                    AuthorUsername = book.Author.UserName
                })
                .ToList();

            var totalCount = query.Count();
            
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(book => book.Title.Contains(value)
                                        || book.Resume.Contains(value)) 
                                         .ToList();
            }

            var filteredCount = query.Count();

            // Sorting
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }

            query = query.OrderBy(orderByString == string.Empty ? "CatalogueNumber asc" : orderByString).ToList();
            
            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length).ToList();

            var data = query.Select(book => new
            {
                Id = book.Id,
                Title = book.Title,
                CatalogueNumber = "<a href=\"./edit/" + book.CatalogueNumber + "\">" + book.CatalogueNumber + "</a>",
                Resume = book.Resume,
                CreatedOn = string.Format("{0:dd/MMM/yyyy}", book.CreatedOn),
                ModifiedOn = string.Format("{0:dd/MMM/yyyy}", book.ModifiedOn),
                isDeleted = book.isDeleted,
                DeletedOn = string.Format("{0:dd/MMM/yyyy}", book.DeletedOn),
                AuthorUsername = book.AuthorUsername
            }).ToList();

            return Json(
                new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), 
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            var model = new BookCreateViewModel();
            return View("_CreateBookPartial", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(BookCreateViewModel bookVM)
        {
            User author = usersService.FindSingle(this.User.Identity.Name);
            
            Book book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = bookVM.Title,
                CatalogueNumber = bookVM.CatalogueNumber,
                Resume = bookVM.Resume,
                isDeleted = false,
                DeletedOn = null,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Author = author
            };
            
            try
            {
                var task = this.booksService.Add(book);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to add the Book");
                return View("_CreateBookPartial", bookVM);
            }

            return Content("success");
        }

    }
}