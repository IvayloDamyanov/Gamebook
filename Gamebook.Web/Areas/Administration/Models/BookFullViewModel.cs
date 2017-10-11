using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class BookFullViewModel
    {
        public Guid Id { get; set; }

        public int CatalogueNumber { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorId { get; set; }
    }
}