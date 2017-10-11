using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Models.Page
{
    public class PageDetailedViewModel
    {
        public int BookCatalogueNumber { get; set; }

        public int Number { get; set; }
        
        public string Text { get; set; }

        public ICollection<PageConnectionViewModel> ChildPages { get; set; }
    }
}