using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Models.Book
{
    public class ListViewModel
    {
        public int[] Pages { get; set; }
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public string SearchedTerm { get; set; }
        public ICollection<BookListViewModel> Books { get; set; }
    }
}