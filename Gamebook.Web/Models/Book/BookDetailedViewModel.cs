using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Models.Book
{
    public class BookDetailedViewModel
    {
        public int CatalogueNumber { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }
    }
}