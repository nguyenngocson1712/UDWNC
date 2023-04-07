using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TagBlog.Services.Blogs
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        Task<Category> GetCachedCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Category> GetCategoryByIdAsync(int CategoryId);

        Task<Category> GetCachedCategoryByIdAsync(int CategoryId);

        Task<IList<CategoryItem>> GetCategorysAsync(
            CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategorysAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedCategorysAsync<T>(
            Func<IQueryable<Category>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
            Category Category,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryAsync(
            int CategoryId,
            CancellationToken cancellationToken = default);

        Task<bool> IsCategorySlugExistedAsync(
            int CategoryId, string slug,
            CancellationToken cancellationToken = default);

        
    }
}
