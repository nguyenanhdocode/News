using Application.Models.PostCategory;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all category
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryModel>> GetAll();

        /// <summary>
        /// Get categories has posts
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryModel>> GetHasPostsCategories(List<Guid> excludedPosts);

        /// <summary>
        /// Get max order index
        /// </summary>
        /// <returns></returns>
        Task<int> GetMaxOrderIndex();

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(CreateCategoryModel model);
        
        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category?> GetCategoryById(string id);

        /// <summary>
        /// Get category by slug
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category?> GetCategoryBySlug(string slug);

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdateCategoryModel model);

        /// <summary>
        /// Get active categories
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetActiveCategories();

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        Task Delete(string cateId);

        /// <summary>
        /// Get cates of user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CategoryModel>> GetCatesOfUser(string userId);
    }
}
