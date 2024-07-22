﻿using Application.Models.User;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.PostCategory
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int OrderIndex { get; set; }
        public ProfileModel CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
