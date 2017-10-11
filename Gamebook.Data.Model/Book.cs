using Gamebook.Data.Model.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Data.Model
{
    public class Book : DataModel
    {
        private const int MinTitleLength = 2;
        private const int MaxTitleLength = 100;
        private ICollection<Page> pages;

        public Book()
        {
            this.pages = new HashSet<Page>();
        }

        [Required]
        [MinLength(MinTitleLength)]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }
        
        [Required]
        [Index(IsUnique = true)]
        public int CatalogueNumber { get; set; }

        public string Resume { get; set; }

        public virtual User Author { get; set; }
        
        public virtual ICollection<Page> Pages { get; set; }
    }
}
