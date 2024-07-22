using Application.Models.Post;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostModel, Post>()
                .ForMember(p => p.CoverPhoto, opt => opt.Ignore());
            CreateMap<Post, PostModel>();
            CreateMap<Post, ViewPostModel>();
            CreateMap<Post, UpdatePostModel>()
                .ForMember(p => p.CoverPhoto, opt => opt.Ignore());
            CreateMap<Post, DeletePostModel>();
        }
    }
}
