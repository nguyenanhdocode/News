using Core.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Impl
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public async Task<List<Post>> GetLatestPosts(List<Guid> excludedIds
            , string cateSlug)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);

            if (excludedIds != null)
            {
                predicate = predicate.And(p => !excludedIds.Any(x => x.Equals(p.Id)));
            }

            if (!string.IsNullOrEmpty(cateSlug))
            {
                predicate = predicate.And(p => p.Category.Slug.Equals(cateSlug));
            }

            var posts = await Dbcontext.Posts.Where(predicate)
                .OrderBy(p => p.CreatedOn)
                .Reverse()
                .Take(4)
                .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> GetLatestPostsByCate(Guid cateId, List<Guid> excludedIds)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);

            predicate = predicate.And(p => p.CategoryId.Equals(cateId));

            if (excludedIds != null)
            {
                predicate = predicate.And(p => !excludedIds.Any(x => x.Equals(p.Id)));
            }

            var posts = await Dbcontext.Posts.Where(predicate)
                .OrderBy(p => p.CreatedOn)
                .Reverse()
                .Take(4)
                .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> GetAll(string userId
            , Dictionary<string, string> param)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.Category.IsHidden);

            if (!string.IsNullOrEmpty(userId))
            {
                predicate.And(p => p.CreatedUserId.Equals(userId));
            }

            if (param != null)
            {
                if (param.ContainsKey("Kw"))
                {
                    string kw = param["Kw"] == null ? "" : param["Kw"].ToLower();
                    predicate.And(p => p.Title.ToLower().Contains(kw));
                }

                if (param.ContainsKey("Category") && !param["Category"].Equals("all"))
                {
                    predicate.And(p => p.Category.Slug.Equals(param["Category"]));
                }

                if (param.ContainsKey("Status") && !param["Status"].Equals("all"))
                {
                    predicate.And(p => p.IsDraft == param["Status"].Equals("draft"));
                }
            }
            
            var posts = await Dbcontext.Posts
                .Where(predicate)
                .ToListAsync();

            return posts;
        }

        public async Task<Post?> GetPinnedPost(string cateSlug)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);
            predicate.And(p => p.IsPinned && !p.IsDraft);

            if (!string.IsNullOrEmpty(cateSlug))
            {
                predicate.And(p => p.Category.Slug.Equals(cateSlug));
            }

            var post = await Dbcontext.Posts.Where(predicate)
                .OrderBy(p => p.CreatedOn)
                .Reverse()
                .FirstOrDefaultAsync();
            
            return post;
        }

        public async Task<List<Post>> GetPostsOfCate(string cateId)
        {
            var cate = await Dbcontext.Categories
                .Include(p => p.Posts)
                .SingleOrDefaultAsync(p => p.Id.ToString().Equals(cateId));

            return cate != null ? cate.Posts.ToList() : new List<Post> ();
        }

        public async Task<Post?> GetPostBySlug(string slug)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);
            predicate.And(p => p.Slug.Equals(slug));
            var post = await Dbcontext.Posts.SingleOrDefaultAsync(predicate);

            return post;
        }

        public async Task<List<Post>> GetRelatedPosts(List<Guid> excludedIds, string categorySlug)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);

            if (excludedIds != null)
            {
                predicate = predicate.And(p => !excludedIds.Any(x => x.Equals(p.Id)));
            }

            if (!string.IsNullOrEmpty(categorySlug))
            {
                predicate = predicate.And(p => p.Category.Slug.Equals(categorySlug));
            }

            var posts = await Dbcontext.Posts.Where(predicate)
                .OrderBy(p => p.CreatedOn)
                .Reverse()
                .Take(4)
                .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> Search(Dictionary<string, string> param)
        {
            var predicate = PredicateBuilder.New<Post>(p => !p.IsDraft && !p.Category.IsHidden);

            if (param != null)
            {
                if (param.ContainsKey("SearchKw"))
                {
                    string kw = param["SearchKw"] == null ? "" : param["SearchKw"].ToLower();
                    predicate.And(p => p.Title.ToLower().Contains(kw));
                }
            }

            return await Dbcontext.Posts.Where(predicate).ToListAsync();
        }

        public async Task<List<Post>> GetPostsOfUser(string userId)
        {
            var user = await Dbcontext.Users.Include(p => p.Posts)
                .SingleOrDefaultAsync(p => p.Id.Equals(userId));

            return user != null ? user.Posts.ToList() : new List<Post>();
        }
    }
}
