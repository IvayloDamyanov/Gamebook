using Gamebook.Data.Model.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Data.Model
{
    public class PageConnection: DataModel
    {
        [DefaultValue(0)]
        public int ParentPageNumber { get; set; }

        public string Text { get; set; }

        [DefaultValue(0)]
        public int ChildPageNumber { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        public virtual Book Book { get; set; }
    }
}
