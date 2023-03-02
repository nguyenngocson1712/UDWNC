using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using static TatBlog.Core.Contracts.IPagedList;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPoPularArticlesAsysc(
            int numPost,
            CancellationToken cancellationToken = default);
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsyns(
            int postId,
            CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsyns(
            bool showOnMenu = false,
            CancellationToken CancellationToken = default);
        Task<IList<TagItem>> GetTagesAsyns(
           
           CancellationToken CancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        Task<Tag> GetTagAsync(
            string slug,
        CancellationToken cancellationToken = default);
        
    }
}
