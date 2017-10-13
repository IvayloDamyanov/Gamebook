using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class BookCreateViewModel
    {
        public int CatalogueNumber { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }
    }
}