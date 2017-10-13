using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class PageCreateViewModel
    {
        public int BookCatNum { get; set; }

        public int Number { get; set; }

        public string Text { get; set; }
    }
}