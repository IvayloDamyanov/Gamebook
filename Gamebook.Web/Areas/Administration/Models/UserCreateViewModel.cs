using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class UserCreateViewModel
    {
        public string Role { get;  set;}

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}