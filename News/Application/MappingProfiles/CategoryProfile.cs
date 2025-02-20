﻿using Application.Models.PostCategory;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<Category, UpdateCategoryModel>();
            CreateMap<Category, DeleteCategoryModel>();
        }
    }
}
