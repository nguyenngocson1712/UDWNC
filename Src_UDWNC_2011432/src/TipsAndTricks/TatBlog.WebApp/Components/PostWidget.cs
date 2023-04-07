using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class PostWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public PostWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _blogRepository.GetPoPularArticlesAsysc(3);
            return View(categories);
        }
    }
}
