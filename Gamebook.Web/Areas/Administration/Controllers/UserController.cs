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
    public class UserController : Controller
    {
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [Authorize]
        public ViewResult Edit(string id)
        {
            User user = this.usersService.GetAllAndDeleted().Where(u => u.Id == id).First();

            UserFullViewModel viewModel = new UserFullViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Lockout = user.LockoutEnabled,
                isDeleted = user.isDeleted,
                DeletedOn = user.DeletedOn,
                CreatedOn = user.CreatedOn,
                ModifiedOn = user.ModifiedOn
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserFullViewModel model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            User user = this.usersService.FindSingle(model.UserName);
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.LockoutEnabled = model.Lockout;
            user.ModifiedOn = DateTime.Now;

            if (model.isDeleted && model.DeletedOn == null)
            {
                user.isDeleted = true;
                user.DeletedOn = DateTime.Now;
            }
            if (!model.isDeleted && model.DeletedOn != null)
            {
                user.isDeleted = false;
                user.DeletedOn = null;
            }

            var result = this.usersService.Update(user);
            return this.RedirectToAction("List", "User", new { result = result });
        }

        [Authorize]
        public ActionResult List(int result = 0)
        {
            var viewModel = new ResultViewModel()
            {
                Result = result
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult UserTable([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var query = usersService
                .GetAllAndDeleted()
                .Select(user => new UserFullViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Lockout = user.LockoutEnabled,
                    isDeleted = user.isDeleted,
                    DeletedOn = user.DeletedOn,
                    CreatedOn = user.CreatedOn,
                    ModifiedOn = user.ModifiedOn
                })
                .ToList();

            var totalCount = query.Count();
            
            // Apply filters for searching
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(user => user.UserName.Contains(value)
                                        || user.Email.Contains(value)) 
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

            query = query.OrderBy(orderByString == string.Empty ? "UserName asc" : orderByString).ToList();
            
            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length).ToList();

            var data = query.Select(user => new
            {
                Id = user.Id,
                UserName = "<a href=\"./edit/" + user.Id + "\">" + user.UserName + "</a>",
                Email = user.Email,
                Phone = user.Phone,
                Lockout = user.Lockout,
                CreatedOn = string.Format("{0:dd/MMM/yyyy}", user.CreatedOn),
                ModifiedOn = string.Format("{0:dd/MMM/yyyy}", user.ModifiedOn),
                isDeleted = user.isDeleted,
                DeletedOn = string.Format("{0:dd/MMM/yyyy}", user.DeletedOn)
            }).ToList();

            return Json(
                new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), 
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var model = new UserCreateViewModel();
            return View("_CreateUserPartial", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(UserCreateViewModel userVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("_CreateUserPartial", userVM);
            //}

            User user = new User()
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                EmailConfirmed = true,
                CreatedOn = DateTime.Now
            };
            
            try
            {
                var task = this.usersService.Add(user);
            }
            catch
            {
                ModelState.AddModelError("", "Unable to add the User");
                return View("_CreateUserPartial", userVM);
            }

            return Content("success");
        }

    }
}