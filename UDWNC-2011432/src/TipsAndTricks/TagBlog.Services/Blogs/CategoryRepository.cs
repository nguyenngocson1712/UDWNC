using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<Category> GetCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Category> GetCachedCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"Category.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Category> GetCategoryByIdAsync(int CategoryId)
        {
            return await _context.Set<Category>().FindAsync(CategoryId);
        }

        public async Task<Category> GetCachedCategoryByIdAsync(int CategoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"Category.by-id.{CategoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryByIdAsync(CategoryId);
                });
        }

        public async Task<IList<CategoryItem>> GetCategorysAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .OrderBy(a => a.Name)
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ShowOnMenu = a.ShowOnMenu,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        //public async Task<IPagedList<CategoryItem>> GetPagedCategorysAsync(
        //    IPagingParams pagingParams,
        //    string name = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Category>()
        //        .AsNoTracking()
        //        //.Whereif(!string.IsNullOrWhiteSpace(name),
        //        //    x => x.FullName.Contains(name))
        //        .Select(a => new CategoryItem()
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

        public async Task<IPagedList<CategoryItem>> GetPagedCategorysAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> CategoryQuery = _context.Set<Category>().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
            {
                CategoryQuery = CategoryQuery.Where(x => x.Name.Contains(name));
            }
            return await CategoryQuery.Select(a => new CategoryItem()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                UrlSlug = a.UrlSlug,
                PostCount = a.Posts.Count(p => p.Published)
            })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedCategorysAsync<T>(
            Func<IQueryable<Category>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            var CategoryQuery = _context.Set<Category>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                CategoryQuery = CategoryQuery.Where(x => x.Name.Contains(name));
            }

            return await mapper(CategoryQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(
            Category Category, CancellationToken cancellationToken = default)
        {
            if (Category.Id > 0)
            {
                _context.Categories.Update(Category);
                _memoryCache.Remove($"Category.by-id.{Category.Id}");
            }
            else
            {
                _context.Categories.Add(Category);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryAsync(
            int CategoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(x => x.Id == CategoryId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsCategorySlugExistedAsync(
            int CategoryId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(x => x.Id != CategoryId && x.UrlSlug == slug, cancellationToken);
        }

    }
 }
