using Application.Exceptions;
using Application.Models.PostCategory;
using AutoMapper;
using Core.Entities;
using DataAccess.UnifOfWork;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private IClaimService _claimService;

        public CategoryService(IUnitOfWork uow
            , IMapper mapper
            , IClaimService claimService)
        {
            _uow = uow;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task Create(CreateCategoryModel model)
        {
            var cate = _mapper.Map<CreateCategoryModel, Category>(model);
            cate.Slug = cate.Slug.ToLower();
            cate.CreatedUserId = _claimService.GetUserId();
            await _uow.CategoryRepository.Add(cate);
            await _uow.SaveChangesAsync();
        }

        public async Task Delete(string cateId)
        {
            var cate = await _uow.CategoryRepository.GetCategoryById(cateId);

            if (cate == null)
                throw new NotFoundException("Category not found");

            var posts = await _uow.PostRepository.GetPostsOfCate(cateId);
            if (posts.Count() > 0)
                throw new UnprocessableEntityException("Category contain posts");

            _uow.CategoryRepository.Delete(cate);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<Category>> GetActiveCategories()
        {
            return await _uow.CategoryRepository.GetActiveCategories();
        }

        public async Task<List<CategoryModel>> GetAll()
        {
            var cates = (await _uow.CategoryRepository.GetAllAsync())
                .OrderBy(p => p.OrderIndex)
                .ToList();
            return _mapper.Map<List<Category>, List<CategoryModel>>(cates);
        }

        public async Task<Category?> GetCategoryById(string id)
        {
            return await _uow.CategoryRepository.GetCategoryById(id);
        }

        public async Task<Category?> GetCategoryBySlug(string slug)
        {
            return await _uow.CategoryRepository.GetCategoryBySlug(slug);
        }

        public async Task<List<CategoryModel>> GetCatesOfUser(string userId)
        {
            var cates = await _uow.CategoryRepository.GetCatesOfUser(userId);

            return _mapper.Map<List<Category>, List<CategoryModel>>(cates.ToList());
        }

        public async Task<List<CategoryModel>> GetHasPostsCategories(List<Guid> excludedPosts)
        {
            var posts = await _uow.CategoryRepository
                .GetHasPostsCategories(excludedPosts);

            return _mapper.Map<List<Category>, List<CategoryModel>>(posts.ToList());
        }

        public async Task<int> GetMaxOrderIndex()
        {
            return await _uow.CategoryRepository.GetMaxOrderIndex();
        }

        public async Task Update(UpdateCategoryModel model)
        {
            var cate = await _uow.CategoryRepository.GetCategoryById(model.Id.ToString());
            if (cate == null)
                throw new NotFoundException("Category not found");

            if (!cate.CreatedUserId.Equals(_claimService.GetUserId()))
                throw new ForbiddenException();

            cate.Name = model.Name;
            cate.Slug = model.Slug;
            cate.OrderIndex = model.OrderIndex;
            cate.IsHidden = model.IsHidden;

            _uow.CategoryRepository.Update(cate);
            await _uow.SaveChangesAsync();
        }
    }
}
