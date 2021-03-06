﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamebook.Web.Areas.Administration.Models
{
    public class UserFullViewModel
    {
        public string Id { get; set; }

        public string Role { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool Lockout { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}