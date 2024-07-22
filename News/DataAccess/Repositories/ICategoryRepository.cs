using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetActiveCategories();
        Task<List<Category>> GetHasPostsCategories(List<Guid> excludedPostIds);
        Task<int> GetMaxOrderIndex();
        Task<Category?> GetCategoryById(string id);
        Task<Category?> GetCategoryBySlug(string slug);
        Task<List<Category>> GetCatesOfUser(string userId);
    }
}
