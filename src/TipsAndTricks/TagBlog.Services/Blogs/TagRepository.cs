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

namespace TagBlog.Services.Blogs
{
    //public class TagRepository : ITagRepository
    //{
    //    private readonly BlogDbContext _context;
    //    private readonly IMemoryCache _memoryCache;

    //    public TagRepository(BlogDbContext context, IMemoryCache memoryCache)
    //    {
    //        _context = context;
    //        _memoryCache = memoryCache;
    //    }

    //    public async Task<Tag> GetTagBySlugAsync(
    //        string slug, CancellationToken cancellationToken = default)
    //    {
    //        return await _context.Set<Tag>()
    //            .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
    //    }

    //    public async Task<Tag> GetCachedTagBySlugAsync(
    //        string slug, CancellationToken cancellationToken = default)
    //    {
    //        return await _memoryCache.GetOrCreateAsync(
    //            $"Tag.by-slug.{slug}",
    //            async (entry) =>
    //            {
    //                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
    //                return await GetTagBySlugAsync(slug, cancellationToken);
    //            });
    //    }

    //    public async Task<Tag> GetTagByIdAsync(int TagId)
    //    {
    //        return await _context.Set<Tag>().FindAsync(TagId);
    //    }

    //    public async Task<Tag> GetCachedTagByIdAsync(int TagId)
    //    {
    //        return await _memoryCache.GetOrCreateAsync(
    //            $"Tag.by-id.{TagId}",
    //            async (entry) =>
    //            {
    //                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
    //                return await GetTagByIdAsync(TagId);
    //            });
    //    }

    //    public async Task<IList<TagItem>> GetTagsAsync(
    //        CancellationToken cancellationToken = default)
    //    {
    //        return await _context.Set<Tag>()
    //            .OrderBy(a => a.Name)
    //            .Select(a => new TagItem()
    //            {
    //                Id = a.Id,
    //                Name = a.Name,
    //                Description = a.Description,
    //               ,
    //                UrlSlug = a.UrlSlug,
    //                PostCount = a.Posts.Count(p => p.Published)
    //            })
    //            .ToListAsync(cancellationToken);
    //    }

    //    //public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
    //    //    IPagingParams pagingParams,
    //    //    string name = null,
    //    //    CancellationToken cancellationToken = default)
    //    //{
    //    //    return await _context.Set<Tag>()
    //    //        .AsNoTracking()
    //    //        //.Whereif(!string.IsNullOrWhiteSpace(name),
    //    //        //    x => x.FullName.Contains(name))
    //    //        .Select(a => new TagItem()
    //    //        {
    //    //            Id = a.Id,
    //    //            FullName = a.FullName,
    //    //            Email = a.Email,
    //    //            JoinedDay = a.JoinedDay,
    //    //            ImageUrl = a.ImageUrl,
    //    //            UrlSlug = a.UrlSlug,
    //    //            PostCount = a.Posts.Count(p => p.Published)
    //    //        })
    //    //        .ToPagedListAsync(pagingParams, cancellationToken);
    //    //}

    //    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
    //        IPagingParams pagingParams,
    //        string name = null,
    //        CancellationToken cancellationToken = default)
    //    {
    //        IQueryable<Tag> TagQuery = _context.Set<Tag>().AsNoTracking();
    //        if (!string.IsNullOrWhiteSpace(name))
    //        {
    //            TagQuery = TagQuery.Where(x => x.Name.Contains(name));
    //        }
    //        return await TagQuery.Select(a => new TagItem()
    //        {
    //            Id = a.Id,
    //            Name = a.Name,
    //            Description = a.Description,
    //            UrlSlug = a.UrlSlug,
    //            PostCount = a.Posts.Count(p => p.Published)
    //        })
    //            .ToPagedListAsync(pagingParams, cancellationToken);
    //    }

    //    public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
    //        Func<IQueryable<Tag>, IQueryable<T>> mapper,
    //        IPagingParams pagingParams,
    //        string name = null,
    //        CancellationToken cancellationToken = default)
    //    {
    //        var TagQuery = _context.Set<Tag>().AsNoTracking();

    //        if (!string.IsNullOrEmpty(name))
    //        {
    //            TagQuery = TagQuery.Where(x => x.Name.Contains(name));
    //        }

    //        return await mapper(TagQuery)
    //            .ToPagedListAsync(pagingParams, cancellationToken);
    //    }

    //    public async Task<bool> AddOrUpdateAsync(
    //        Tag Tag, CancellationToken cancellationToken = default)
    //    {
    //        if (Tag.Id > 0)
    //        {
    //            _context.Categories.Update(Tag);
    //            _memoryCache.Remove($"Tag.by-id.{Tag.Id}");
    //        }
    //        else
    //        {
    //            _context.Categories.Add(Tag);
    //        }

    //        return await _context.SaveChangesAsync(cancellationToken) > 0;
    //    }

    //    public async Task<bool> DeleteTagAsync(
    //        int TagId, CancellationToken cancellationToken = default)
    //    {
    //        return await _context.Categories
    //            .Where(x => x.Id == TagId)
    //            .ExecuteDeleteAsync(cancellationToken) > 0;
    //    }

    //    public async Task<bool> IsTagSlugExistedAsync(
    //        int TagId,
    //        string slug,
    //        CancellationToken cancellationToken = default)
    //    {
    //        return await _context.Categories
    //            .AnyAsync(x => x.Id != TagId && x.UrlSlug == slug, cancellationToken);
    //    }

    //}
}
