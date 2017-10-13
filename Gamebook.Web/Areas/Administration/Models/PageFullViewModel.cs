using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class PageFullViewModel
    {
        public Guid Id { get; set; }

        public int BookCatNum { get; set; }

        public int Number { get; set; }

        public string Text { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorId { get; set; }
    }
}