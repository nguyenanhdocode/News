using Application.Models.Post;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IPostService
    {
        /// <summary>
        /// Create post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(CreatePostModel model);

        /// <summary>
        /// Get post by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<PostModel>> GetAll(string userId, Dictionary<string, string> param);

        /// <summary>
        /// Get post by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<ViewPostModel> GetPostBySlug(string slug);

        /// <summary>
        /// Get hot post
        /// </summary>
        /// <returns></returns>
        Task<ViewPostModel> GetPinnedPost(string cateSlug);

        /// <summary>
        /// Get latest post
        /// </summary>
        /// <returns></returns>
        Task<List<ViewPostModel>> GetLatestPosts(List<Guid> excludedIds, string cateSlug);

        /// <summary>
        /// Get latest posts by cate
        /// </summary>
        /// <returns></returns>
        Task<List<ViewPostModel>> GetLatestPostsByCate(Guid cateId, List<Guid> excludedIds);

        /// <summary>
        /// Get related posts
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <returns></returns>
        Task<List<ViewPostModel>> GetRelatedPosts(List<Guid> excludedIds, string categorySlug);

        /// <summary>
        /// Increase viewcounter
        /// </summary>
        /// <returns></returns>
        Task IncreaseViewCounter(Guid postId);

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<Post?> GetPostById(string postId);

        /// <summary>
        /// Update post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdatePostModel model);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task Delete(string postId);

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ViewPostModel>> Search(Dictionary<string, string> param);

        /// <summary>
        /// Get posts of category
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        Task<List<ViewPostModel>> GetPostsOfCate(string cateId);

        /// <summary>
        /// Get posts of user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ViewPostModel>> GetPostsOfUser(string userId);
    }
}
