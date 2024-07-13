﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.User
{
    public class RegisterModel
    {
        public string Email {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
