using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PostsBlog.Services.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Extensions;

namespace TagBlog.Services.Blogs
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public PostRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<Post> GetPostBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Post> GetCachedPostBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"Post.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetPostBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Post> GetPostByIdAsync(int PostId)
        {
            return await _context.Set<Post>().FindAsync(PostId);
        }

        public async Task<Post> GetCachedPostByIdAsync(int PostId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"Post.by-id.{PostId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetPostByIdAsync(PostId);
                });
        }

        public async Task<IList<PostItem>> GetPostsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(a => a.Title)
                .Select(a => new PostItem()
                {
                    Id = a.Id,
                    Title =  a.Title,
                   
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    //PostCount = a.ViewCount.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        //public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
        //    IPagingParams pagingParams,
        //    string name = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Post>()
        //        .AsNoTracking()
        //        //.Whereif(!string.IsNullOrWhiteSpace(name),
        //        //    x => x.FullName.Contains(name))
        //        .Select(a => new PostItem()
        //        {
        //            Id = a.Id,
        //            FullName = a.FullName,
        //            Email = a.Email,
        //            JoinedDay = a.JoinedDay,
        //            ImageUrl = a.ImageUrl,
        //            UrlSlug = a.UrlSlug,
        //            PostCount = a.Posts.Count(p => p.Published)
        //        })
        //        .ToPagedListAsync(pagingParams, cancellationToken);
        //}

        public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> PostQuery = _context.Set<Post>().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
            {
                PostQuery = PostQuery.Where(x => x.Title.Contains(name));
            }
            return await PostQuery.Select(a => new PostItem()
            {
                Id = a.Id,
                Title = a.Title,
               
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                //PostCount = a.Post.Count(p => p.Published)
            })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            var PostQuery = _context.Set<Post>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                PostQuery = PostQuery.Where(x => x.Title.Contains(name));
            }

            return await mapper(PostQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(
            Post Post, CancellationToken cancellationToken = default)
        {
            if (Post.Id > 0)
            {
                _context.Posts.Update(Post);
                _memoryCache.Remove($"Post.by-id.{Post.Id}");
            }
            else
            {
                _context.Posts.Add(Post);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeletePostAsync(
            int PostId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(x => x.Id == PostId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int PostId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .AnyAsync(x => x.Id != PostId && x.UrlSlug == slug, cancellationToken);
        }

        
    }
}
