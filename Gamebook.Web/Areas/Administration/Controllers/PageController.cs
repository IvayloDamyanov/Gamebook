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
    public class PageController : Controller
    {
        private readonly IBooksService booksService;
        private readonly IPagesService pagesService;
        private readonly IPageConnectionsService pageConnectionsService;
        private readonly IUsersService usersService;

        public PageController(
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

        //// GET: \page - main page + search
        //[HttpGet]
        //[Authorize]
        //public ViewResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id, int bookNum)
        {
            Page page = this.pagesService.Find(bookNum, id);

            PageFullViewModel viewModel = new PageFullViewModel()
            {
                Id = page.Id,
                BookCatNum = page.Book.CatalogueNumber,
                Number = page.Number,
                Text = page.Text,
                isDeleted = page.isDeleted,
                DeletedOn = page.DeletedOn,
                CreatedOn = page.CreatedOn,
                ModifiedOn = page.ModifiedOn,
                AuthorUsername = page.Author.UserName,
                AuthorId = page.Author.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(PageFullViewModel model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            Page page = this.pagesService.Find(model.BookCatNum, model.Number);
            page.Text = model.Text;
            page.Number = model.Number;
            page.ModifiedOn = DateTime.Now;

            if (model.isDeleted && model.DeletedOn == null)
            {
                page.isDeleted = true;
                page.DeletedOn = DateTime.Now;
            }
            if (!model.isDeleted && model.DeletedOn != null)
            {
                page.isDeleted = false;
                page.DeletedOn = null;
            }

            var result = this.pagesService.Update(page);
            return this.RedirectToAction("List", "Page", new { result = result });
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
        public ActionResult PageTable([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var query = pagesService
                .GetAllAndDeleted()
                .Select(page => new PageFullViewModel()
                {
                    Id = page.Id,
                    BookCatNum = page.Book.CatalogueNumber,
                    Number = page.Number,
                    Text = page.Text,
                    isDeleted = page.isDeleted,
                    DeletedOn = page.DeletedOn,
                    CreatedOn = page.CreatedOn,
                    ModifiedOn = page.ModifiedOn,
                    AuthorUsername = page.Author.UserName,
                    AuthorId = page.Author.Id
                })
                .ToList();

            var totalCount = query.Count();
            
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(page => page.Text.Contains(value)).ToList();
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

            query = query.OrderBy(orderByString == string.Empty ? "Number asc" : orderByString).ToList();
            
            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length).ToList();

            var data = query.Select(page => new
            {
                Id = page.Id,
                //page / edit ? id = 13 & bookNum = 1
                Number = "<a href=\"./edit?id=" + page.Number + "&bookNum=" + page.BookCatNum + "\">" + page.Number + "</a>",
                BookCatNum = page.BookCatNum,
                Text = page.Text,
                CreatedOn = string.Format("{0:dd/MMM/yyyy}", page.CreatedOn),
                ModifiedOn = string.Format("{0:dd/MMM/yyyy}", page.ModifiedOn),
                isDeleted = page.isDeleted,
                DeletedOn = string.Format("{0:dd/MMM/yyyy}", page.DeletedOn),
                AuthorUsername = page.AuthorUsername,
                AuthorId = page.AuthorId
            }).ToList();

            return Json(
                new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), 
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            var model = new PageCreateViewModel();
            return View("_CreatePagePartial", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(PageCreateViewModel pageVM)
        {
            if (!ModelState.IsValid)
            {
                return View("_CreatePagePartial", pageVM);
            }


            User author = usersService.FindSingle(this.User.Identity.Name);
            Book book = booksService.FindSingle(pageVM.BookCatNum);
            
            Page page = new Page()
            {
                Id = Guid.NewGuid(),
                Number = pageVM.Number,
                Text = pageVM.Text,
                isDeleted = false,
                DeletedOn = null,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Book = book,
                Author = author
            };

            try
            {
                var task = this.pagesService.Add(page);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to add the Page");
                return View("_CreatePagePartial", pageVM);
            }

            return Content("success");
        }

    }
}