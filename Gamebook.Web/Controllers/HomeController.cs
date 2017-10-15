using Gamebook.Services.Contracts;
using Gamebook.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamebook.Web.Controllers
{
    public class HomeController : Controller
    {
        private IBooksService booksService;
        private IPagesService pagesService;

        public HomeController(IBooksService booksService, IPagesService pagesService)
        {
            this.booksService = booksService;
            this.pagesService = pagesService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ViewResult Error()
        {
            return View();
        }

        [OutputCache(Duration = 3600)]
        [ChildActionOnly]
        public PartialViewResult Stats()
        {
            int booksCount = booksService.GetAll().Count();
            int pagesCount = pagesService.GetAll().Count();

            var model = new StatsViewModel()
            {
                BooksCount = booksCount,
                PagesCount = pagesCount
            };

            return this.PartialView("_StatsViewPartial", model);
        }
    }
}