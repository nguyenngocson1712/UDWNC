using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers

{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            return View();
        }
        //private readonly IBlogRepository _blogRepository;
        //public BlogController(IBlogRepository blogRepository)
        //{
        //    _blogRepository = blogRepository;
        //}


        //public async Task<IActionResult> Index(

        //    [FromQuery(Name = "p")] int pageNumber = 1,
        //        [FromQuery(Name = "ps")] int pageSize = 10)
        //{
        //    var postQuery = new PostQuery()
        //    {
        //        PublishedOnly = true
        //    };
        //    var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

        //    ViewBag.PostQuery = postQuery;

        //    return View(postList);
        //}
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("nội dung sẽ cập nhật");

    }
}
