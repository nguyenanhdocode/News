﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.PostCategory
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int OrderIndex { get; set; }
    }
}
