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
       // Task<IPagedList< Post>> GetPagedPostAsync(
       //     PostQuery condition,
       //int pageNumber=1,
       //int pageSize=10,
       
       //CancellationToken cancellationToken = default);
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
        Task<Category> GetCategoriesAsync(
          string slug,
      CancellationToken cancellationToken = default);
        Task<bool> DeleteTagByIdAsync(int id,CancellationToken cancellationToken=default);
        Task<bool> IsSlugCategoryAsync(string slug, CancellationToken cancellationToken=default);
        Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Object> CountByMostMonthAsync(int month,CancellationToken cancellationToken=default);
        Task<Category> FindCategoriesByIdAsync(int id, CancellationToken cancellationToken = default);
        Task <IList<Post>> FindPostByPostQueryAsync(PostQuery query, CancellationToken cancellationToken=default);
        //Task<IPagedList<Post>> GetPageFindPostByPostQueryAsync( IPagingParams pagingParams,PostQuery query, CancellationToken cancellationToken = default);
        Task ChangeSTTPost(int id, CancellationToken cancellationToken = default);
        Task <bool> AddorUpPostAsync( Post post, CancellationToken cancellationToken = default);
        Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);
        Task<IPagedList<Post>> GetPagePostsAsync(PostQuery condition,int pageNumber,int pageSize,CancellationToken cancellationToken=default);
        
    }
}
