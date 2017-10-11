using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamebook.Data.Model.Contracts
{
    public interface IDeletable
    {
        bool isDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
