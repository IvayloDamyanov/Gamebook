using Gamebook.Services.Contracts;
using Gamebook.Web.Areas.Administration.Models;
using System.Web.Mvc;

namespace Gamebook.Web.Areas.Administration.Controllers
{
    [RouteArea("Administration")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Result(int result)
        {
            var model = new ResultViewModel()
            {
                Result = result
            };

            return View(model);
        }
    }
}