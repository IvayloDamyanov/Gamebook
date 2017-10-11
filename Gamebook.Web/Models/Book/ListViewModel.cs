using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Models.Book
{
    public class ListViewModel
    {
        public ICollection<BookListViewModel> Books { get; set; }
    }
}