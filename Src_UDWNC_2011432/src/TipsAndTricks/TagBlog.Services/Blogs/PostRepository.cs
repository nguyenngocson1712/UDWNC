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
                    Title = a.Title,

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


        public async Task<IPagedList<T>> GetPagedPostAsync<T>(PostQuery pq, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            var posts = FilterPost(pq);
            var mapperPosts = mapper(posts);
            return await mapperPosts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public IQueryable<Post> FilterPost(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Tags);
            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.SlugCategory))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.SlugCategory);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }

        public Task<bool> SetImageUrlAsync(int PostId, string imageUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

