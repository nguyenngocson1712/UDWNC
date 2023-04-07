using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers

{
    public class BlogController : Controller
    {
        //public IActionResult Index()
        //{
        //    ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        //    return View();
        //}
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
                [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword= keyword
            };
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }
        public async Task<IActionResult> Category(
           [FromRoute(Name = "slug")] string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
               [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                SlugCategory = slug
            };
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }
        public async Task<IActionResult> Tag(
           [FromRoute(Name = "slug")] string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
               [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                TagSlug = slug
            };
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }
        public async Task<IActionResult> Author(
          [FromRoute(Name = "slug")] string slug,
          [FromQuery(Name = "p")] int pageNumber = 1,
              [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                AuthorSlug = slug
            };
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public async Task<IActionResult> PostInfo( 
          [FromQuery(Name = "year")] int year,
          [FromQuery(Name = "month")] int month,
          [FromRoute(Name = "slug")] string slug,
            int pageNumber = 1,
              int pageSize = 5)
        {
            
            var postList = await _blogRepository.GetPostAsync( year, month, slug);

          

            return View(postList);
        }
        public async Task<IActionResult> Archives(
            [FromQuery(Name = "year")] int year, 
            [FromQuery(Name = "month")] int month,
            int pageNumber=1, int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                PostedMonth = month,
                PostedYear=year
                

            };
            var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postList);
        }
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("nội dung sẽ cập nhật");

    }
}
