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
    public class Page : DataModel
    {
        private const int maxTextLength = 1000;
        
        [Required]
        [DefaultValue(0)]
        public int Number { get; set; }

        [MaxLength(maxTextLength)]
        public string Text { get; set; }
        
        public virtual User Author { get; set; }
        
        public virtual Book Book { get; set; }
    }
}
