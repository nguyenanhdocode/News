using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post?> GetPinnedPost(string cateSlug);
        Task<List<Post>> GetLatestPosts(List<Guid> excludedIds, string cateSlug);
        Task<List<Post>> GetLatestPostsByCate(Guid cateId, List<Guid> excludedIds);
        Task<List<Post>> GetAll(string userId, Dictionary<string, string> param);
        Task<Post?> GetPostBySlug(string slug);
        Task<List<Post>> GetRelatedPosts(List<Guid> excludedIds, string categorySlug);
        Task<List<Post>> Search(Dictionary<string, string> param);
        Task<List<Post>> GetPostsOfCate(string cateId);
        Task<List<Post>> GetPostsOfUser(string userId);
    }
}
