﻿using TatBlog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Core.DTO;
using TatBlog.Core.Contracts;
using TatBlog.Services.Extensions;
using System.Threading;
using TagBlog.Services.Blogs;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository

    {
        private readonly BlogDbContext _context;
        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
       CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);
            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }
            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IList<Post>> GetPoPularArticlesAsysc(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)

        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        public async Task IncreaseViewCountAsyns(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        //public async Task<IList<CategoryItem>> GetCategoriesAsyns(bool showOnMenu = false, CancellationToken CancellationToken = default)
        //{
        //    IQueryable<Category> categories = _context.Set<Category>();
        //    if (showOnMenu)
        //    {
        //        categories = categories.Where(x => x.ShowOnMenu);

        //    }
        //    return await categories
        //        .OrderBy(x => x.Name)
        //        .Select(x => new CategoryItem()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            UrlSlug = x.UrlSlug,
        //            Description = x.Description,
        //            ShowOnMenu = x.ShowOnMenu,
        //            PostCount = x.Posts.Count(p => p.Published)

        //        })
        //        .ToListAsync(CancellationToken);
        //}

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
             .Select(x => new TagItem()
             {
                 Id = x.Id,
                 Name = x.Name,
                 UrlSlug = x.UrlSlug,
                 Description = x.Description,
                 PostCount = x.Posts.Count(p => p.Published)

             });
            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
            
        }

        public async Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(slug)) return null;

            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }
      

        public async Task<IList<TagItem>> GetTagesAsyns(CancellationToken CancellationToken = default)
        {
            IQueryable<Tag> Tages = _context.Set<Tag>();

            return await Tages
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)

                })
                .ToListAsync(CancellationToken);
        }

        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
           return await _context.Set<Tag>()
                .Where(t=>t.Id== id).ExecuteDeleteAsync(cancellationToken)>0;
        }

        public async Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().FindAsync(id, cancellationToken);
        }

        public async Task ChangeSTTPost(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>().Where(p => p.Id == id).ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
        }

        public async Task<Category> FindCategoriesByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().FindAsync(id, cancellationToken);
        }

        public async Task<Category> GetCategoriesBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(slug)) return null;

            return await _context.Set<Category>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> IsSlugCategoryAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category> ().AnyAsync(c=>c.UrlSlug.Equals(slug),cancellationToken);
        }

        

        public async Task<bool> AddorUpPostAsync(Post post, CancellationToken cancellationToken = default)
        {
           _context.Entry(post).State=post.Id==0?EntityState.Added:EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken)>0;
        }

        public Task<object> CountByMostMonthAsync(int month, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        

        public  async Task<IList<Post>>  FindPostByPostQueryAsync(PostQuery query,CancellationToken cancellationToken=default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Where(
                p => p.AuthorId == query.AuthorId
               || p.CategoryId == query.CategoryId
               || p.Category.UrlSlug.Equals(query.SlugCategory)
               || p.PostedDate.Month == query.TimeCreated.Month
               || p.PostedDate.Year == query.TimeCreated.Year
               || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag))).ToListAsync(cancellationToken);
        }

        //public async Task<IPagedList<Post>> GetPageFindPostByPostQueryAsync(IPagingParams pagingParams, PostQuery query, CancellationToken cancellationToken = default)
        //{
        //    var posts = _context.Set<Post>()
        //                    .Include(a => a.Author)
        //                    .Include(c => c.Category)
        //                    .Where(
        //                        p => p.AuthorId == query.AuthorId
        //                        || p.CatogoryId == query.CategoryId
        //                        || p.Category.UrlSlug.Equals(query.SlugCategory)
        //                        || p.PostedDate.Month == query.TimeCreated.Month
        //                        || p.PostedDate.Year == query.TimeCreated.Year
        //                        || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag)));

        //    return await posts.ToPagedListAsync(pagingParams, cancellationToken);

        //}

        private IQueryable<Post> FilterPosts(PostQuery condition)
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
            public async Task<IPagedList<Post>> GetPagePostsAsync(PostQuery condition, int pageNumber=1, int pageSize=10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
           pageNumber, pageSize, nameof(Post.PostedDate),"DESC" ,cancellationToken);
        }

        public async Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default)
        {
            _context.Entry(newCategory).State = newCategory.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderBy(a => a.FullName)
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.ToString(),
                    JoinedDay = a.JoinedDay,
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    Notes = a.Notes,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }



        public async Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
        PostQuery condition,
        IPagingParams pagingParams,
        Func<IQueryable<Post>, IQueryable<T>> mapper)
        {
            var posts = FilterPosts(condition);
            var projectedPosts = mapper(posts);

            return await projectedPosts.ToPagedListAsync(pagingParams);
        }

        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }

        public async Task<Post> CreateOrUpdatePostAsync(
         Post post, IEnumerable<string> tags,
         CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }

        public async Task<IList<TagItem>> GetListTagItemAsync(TagQuery condition, CancellationToken cancellationToken = default)
        {
            var tagItems = _context.Set<Tag>().Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UrlSlug = x.UrlSlug,
                PostCount = x.Posts.Count(p => p.Published)
            });

            if (condition != null)
            {
                if (!string.IsNullOrWhiteSpace(condition.KeyWord))
                {
                    tagItems = tagItems.Where(x => x.Name.Contains(condition.KeyWord) ||
                                              x.Description.Contains(condition.KeyWord));
                }
            }

            return await tagItems.ToListAsync(cancellationToken);
        }

        public async Task<Tag> FindTagById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> AddOrEditTagAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            _context.Entry(tag).State = tag.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }













        //public async Task<IPagedList<Post>> GetPagedPostAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        //{
        //   return await FilterPost(condition).ToPagedListAsync(
        //       pageNumber, pageSize, nameof(Post.PostedDate),"DESC" ,cancellationToken);
        //}
    }
}


