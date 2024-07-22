using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Impl
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public async Task<List<Category>> GetActiveCategories()
        {
            return await Dbcontext.Categories.Where(p => !p.IsHidden).ToListAsync();
        }

        public async Task<Category?> GetCategoryById(string id)
        {
            return await Dbcontext.Categories
                .SingleOrDefaultAsync(p => p.Id.ToString().Equals(id));
        }

        public async Task<Category?> GetCategoryBySlug(string slug)
        {
            return await Dbcontext.Categories
                            .SingleOrDefaultAsync(p => p.Slug.Equals(slug));
        }

        public async Task<List<Category>> GetCatesOfUser(string userId)
        {
            var user = await Dbcontext.Users.Include(p => p.Categories)
                .SingleOrDefaultAsync(p => p.Id.Equals(userId));

            return user != null ? user.Categories.ToList() : new List<Category>();
        }

        public async Task<List<Category>> GetHasPostsCategories(List<Guid> excludedPostIds)
        {
            var cates = await Dbcontext.Categories.ToListAsync();
            var posts = await Dbcontext.Posts
                .Where(p => !excludedPostIds.Any(x => x.Equals(p.Id))).ToListAsync();

            var result = cates.Join(posts
                , cate => cate.Id
                , post => post.CategoryId
                , (cate, post) => new { cate, post })
            .GroupBy(x => new { x.cate.Id, x.cate.Name, x.cate.Slug })
            .Where(x => x.Count() > 0)
            .Select(x => new Category
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Slug = x.Key.Slug
            })
            .ToList();

            return result;
        }

        public async Task<int> GetMaxOrderIndex()
        {
            var listOrderIndex = await Dbcontext.Categories
                .Select(p => Convert.ToInt32(p.OrderIndex)).ToListAsync();

            return (listOrderIndex != null && listOrderIndex.Count > 0) ? listOrderIndex.Max() : 0;
        }
    }
}
