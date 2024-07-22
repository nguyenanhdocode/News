using Application.Common;
using Application.Exceptions;
using Application.Models.Post;
using AutoMapper;
using Core.Entities;
using DataAccess.UnifOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using MimeKit;
using LinqKit;
using Core.Common;

namespace Application.Services.Impl
{
    public class PostService : IPostService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private IClaimService _claimService;
        private IAssetService _assetService;
        private ICryptographicService _cryptographicService;
        private IRoleService _roleService;

        public PostService(IUnitOfWork uow
            , IMapper mapper
            , IClaimService claimService
            , IAssetService assetService
            , ICryptographicService cryptographicService
            , IRoleService roleService)
        {
            _uow = uow;
            _mapper = mapper;
            _claimService = claimService;
            _assetService = assetService;
            _cryptographicService = cryptographicService;
            _roleService = roleService;
        }

        public async Task Create(CreatePostModel model)
        {
            Guid fileId = await _assetService.Upload(model.CoverPhoto, AssetTypes.Image);
            var post = _mapper.Map<CreatePostModel, Post>(model);
            post.CreatedUserId = _claimService.GetUserId();
            post.CoverPhotoId = fileId;
            post.Content = _cryptographicService.Encrypt(post.Content);
            await _uow.PostRepository.Add(post);
            await _uow.SaveChangesAsync();
        }

        public async Task<ViewPostModel> GetPinnedPost(string cateSlug)
        {
            var post = await _uow.PostRepository.GetPinnedPost(cateSlug);
            return _mapper.Map<Post, ViewPostModel>(post);
        }

        public async Task<List<ViewPostModel>> GetLatestPosts(List<Guid> excludedIds
            , string cateSlug)
        {
            var posts = await _uow.PostRepository.GetLatestPosts(excludedIds, cateSlug);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }

        public async Task<List<ViewPostModel>> GetLatestPostsByCate(Guid cateId
            , List<Guid> excludedIds)
        {
            var posts = await _uow.PostRepository.GetLatestPostsByCate(cateId, excludedIds);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }

        public async Task<List<PostModel>> GetAll(string userId, Dictionary<string, string> param)
        {
            var posts = await _uow.PostRepository.GetAll(userId, param);
            return _mapper.Map<List<Post>, List<PostModel>>(posts);
        }

        public async Task<ViewPostModel> GetPostBySlug(string slug)
        {
            var post = await _uow.PostRepository.GetPostBySlug(slug);

            if (post == null)
                throw new NotFoundException("Post not found");

            var result = _mapper.Map<Post, ViewPostModel>(post);
            result.Content = _cryptographicService.Decrypt(post.Content);

            return result;
        }

        public async Task<List<ViewPostModel>> GetRelatedPosts(List<Guid> excludedIds, string categorySlug)
        {
            var posts = await _uow.PostRepository.GetRelatedPosts(excludedIds, categorySlug);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }

        public async Task IncreaseViewCounter(Guid postId)
        {
            var post = await _uow.PostRepository.Get(p => p.Id.Equals(postId));
            if (post != null)
            {
                post.ViewCount += 1;
                _uow.PostRepository.Update(post);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<Post?> GetPostById(string postId)
        {
            return await _uow.PostRepository.Get(p => p.Id.ToString().Equals(postId));
        }

        public async Task Update(UpdatePostModel model)
        {
            var post = await _uow.PostRepository
                .Get(p => p.Id.ToString().Equals(model.Id));

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.CreatedUserId != _claimService.GetUserId())
                throw new ForbiddenException();

            post.Title = model.Title;
            post.Description = model.Description;
            post.Slug = model.Slug;
            post.CategoryId = Guid.Parse(model.CategoryId);
            post.Content = _cryptographicService.Encrypt(model.Content);
            post.IsDraft = model.IsDraft;
            post.IsPinned = model.IsPinned;
            
            if (model.CoverPhoto != null && model.CoverPhoto.Length > 0)
            {
                Guid fileId = await _assetService.Upload(model.CoverPhoto, AssetTypes.Image);
                post.CoverPhotoId = fileId;
            }

            await _uow.SaveChangesAsync();
        }

        public async Task Delete(string postId)
        {
            var post = await _uow.PostRepository.Get(p => p.Id.ToString().Equals(postId));

            if (post == null)
                throw new NotFoundException("Post not found");

            var userRoles = await _roleService.GetRolesOfUser(_claimService.GetUserId());
            if (post.CreatedUserId != _claimService.GetUserId()
                && !(userRoles.Contains(Roles.Administrator) || userRoles.Contains(Roles.Moderator)))
                throw new ForbiddenException();

            _uow.PostRepository.Delete(post);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<ViewPostModel>> Search(Dictionary<string, string> param)
        {
            var posts = await _uow.PostRepository.Search(param);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }

        public async Task<List<ViewPostModel>> GetPostsOfCate(string cateId)
        {
            var posts = await _uow.PostRepository.GetPostsOfCate(cateId);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }

        public async Task<List<ViewPostModel>> GetPostsOfUser(string userId)
        {
            var posts = await _uow.PostRepository.GetPostsOfUser(userId);

            return _mapper.Map<List<Post>, List<ViewPostModel>>(posts);
        }
    }
}
