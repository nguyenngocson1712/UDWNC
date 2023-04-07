using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Abstractions;
using TagBlog.Services.Media;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController:Controller
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly ILogger<PostsController> _logger;
        public PostsController(IBlogRepository blogRepository,IMapper mapper,IMediaManager mediaManager, ILogger<PostsController> logger)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _mediaManager = mediaManager;
            _logger = logger;
        }

        public async Task< IActionResult> Index(PostFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
                [FromQuery(Name = "ps")] int pageSize = 10)
        {
            _logger.LogInformation("Tạo điều kiện truy vấn");
            var postQuery = _mapper.Map<PostQuery>(model);
            _logger.LogInformation("Lấy danh sách bài viết từ csdl");
            //var postQuery = new PostQuery()
            //{
            //    Keyword = model.Keyword,
            //    CategoryId = model.CategoryId,
            //    AuthorId = model.AuthorId,
            //    Year = model.Year,
            //    Month = model.Month


            //};

            ViewBag.PostsList = await _blogRepository
                .GetPagePostsAsync(postQuery, pageNumber, pageSize);
            _logger.LogInformation("chuẩn bị dữ liệu cho ViewModel");
            await PopulatePostFilterModelAsync(model);
            return View(model);




        }
        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();
            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        private async Task PopulatePostEditModelAsync(PostEditModel model)
        {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();
            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id=0)
        {
            var post = id > 0
                ? await _blogRepository.GetPostByIdAsync(id, true)
                : null;
            var model = post==null
                ? new PostEditModel()
                :_mapper.Map<PostEditModel>(post);

            await PopulatePostEditModelAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit ([FromServices] IValidator< PostEditModel>  postValidator,PostEditModel model)
        {
            var validationResult = await postValidator.ValidateAsync(model);
               if(!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }    

            if (!ModelState.IsValid)
            {
                await PopulatePostEditModelAsync(model);
                return View(model);
            }
            var post =model.Id > 0
                ? await _blogRepository.GetPostByIdAsync(model.Id)
                : null;
            if(post==null)
            {
                post = _mapper.Map<Post>(model);
                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }    
            else
            {
                _mapper.Map(model,post);
                post.Category = null;
                post.ModifiedDate= DateTime.Now;
            }  
            if(model.ImageFile?.Length>0)
            {
                var newImagePath = await _mediaManager.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(newImagePath))
                
                {
                    await _mediaManager.DeleteFileASync(post.ImageUrl);
                    post.ImageUrl= newImagePath;
                }

            }    
            
            await _blogRepository.CreateOrUpdatePostAsync(post,model.GetSelectedTags());
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(int id, string urlSLug)
        {
            var slugExisted=await _blogRepository
                .IsPostSlugExistedAsync(id,urlSLug);
            return slugExisted
                ? Json($"Slug'{urlSLug}' đã được sử dụng")
                : Json(true);
        }
    }

    }

