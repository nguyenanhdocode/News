﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.User
{
    public class UpdateRolesModel
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
