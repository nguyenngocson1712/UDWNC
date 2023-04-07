
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PostsBlog.Services.Blogs;
using System.Net;
using System.Net.NetworkInformation;
using TagBlog.Services.Blogs;
using TagBlog.Services.Media;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;


namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        //public static WebApplication MapPostEndpoints(this WebApplication app)
        //{
        //    var routeGroupBuilder = app.MapGroup("/api/Posts");
        //    routeGroupBuilder.MapGet("/", GetPosts)
        //        .WithName("GetPosts")
        //        .Produces<PaginationResult<PostItem>>();
        //    return app;
        //}
        public static WebApplication MapPostEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");
            routeGroupBuilder.MapGet("/", GetPosts)
                .WithName("GetPosts")
            .Produces<PaginationResult<PostQuery>>();
            //    .Produces<ApiResponse<PaginationResult<PostDto>>>();
            //.Produces<PaginationResult<PostItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostByPostId)
                .WithName("GetPostsByPostId")
                .Produces<PostItem>()
                .Produces(404);


            //routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
            //    .WithName("GetRandomPosts")
            //    .Produces<ApiResponse<IList<PostDto>>>();

            //routeGroupBuilder.MapGet("/{id:int}", GetPostByPostId)
            //    .WithName("GetPostById")
            //    .Produces<ApiResponse<PostDetail>>();
            //.Produces<PostItem>()
            //.Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9 -]+$)}/Post", GetPostByPostId)
               .WithName("GetPostByPostSlug")
               .Produces<PaginationResult<PostDto>>();
            //.Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                //.RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<PostItem>>();
            //.Produces(201)
            //.Produces(400)
            //.Produces<ApiResponse<string>>();
            //.Produces(409);

            routeGroupBuilder.MapPost("/{id:int}/avtar", SetImagePost)
                .WithName("SetPostPicture")
                .Accepts<IFormFile>("mutipart/form-data")
            //    .Produces(401)
            //    .RequireAuthorization()
               .Produces<ApiResponse<string>>();
            //.Produces<string>
            //.Produces(400);

            routeGroupBuilder.MapPost("/{id:int}", UpdatePost)
                .WithName("UpdateAnPost")
                .RequireAuthorization()
                .Produces(401)
           .AddEndpointFilter<ValidatorFilter<PostEditModel>>();
            //.Produces(204)
            //.Produces(400)
            //.Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                //.RequireAuthorization()
                //.Produces(401)
                .Produces<ApiResponse<string>>();
            //.Produces(204)
            //.Produces(404);



            return app;
        }
        private static async Task<IResult> GetPosts(
             [AsParameters] PostFilterModel model,
             IPostRepository postRepository,
               IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postsList = await postRepository.GetPagedPostAsync(
              postQuery,model,
              posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(paginationResult);
        }
        //private static async Task<IResult> GetPopularArticle(int limit, IBlogRepository blogRepository, IMapper mapper)
        //{
        //    var posts = await blogRepository.GetPopularArticleAsync(limit);


        //    return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        //}
        //private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper)
        //{
        //    var posts = await blogRepository.GetRandomArticlesAsync(limit);

        //    return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        //}

        //private static async Task<IResult> GetPostsInMonthly(
        //    int limit,
        //    IBlogRepository blogRepository)
        //{
        //    var posts = await blogRepository.CountMonthlyPostsAsync(limit);
        //    return Results.Ok(ApiResponse.Success(posts));
        //}


        public static async Task<IResult> GetPostByPostId(
            int id,
            [AsParameters] PagingModel pagingModel,
           IMapper mapper,
            IPostRepository postRepository)
        {
            //var post = await postRepository.GetPostByIdAsync(id, true);
            //return post == null
            //              ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy id = {id}"))
            //              : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
            var postQuery = new PostQuery()
            {
                PostId = id,
                PublishedOnly = true,
            };
            var postsList = await postRepository.GetPagedPostAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }
        private static async Task<IResult> GetPostDetailBySlug([FromRoute] string slug, IBlogRepository blogRepository,IPostRepository postRepository, IMapper mapper, [AsParameters] PagingModel pagingModel)
        {
            //var post = await blogRepository.GetPostBySlugAsync(slug, true);

            //return post == null
            //    ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy slug '{slug}'"))
            //    : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
            var postQuery = new PostQuery()
            {
                TagSlug = slug,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }


        private static async Task<IResult> AddPost(
             PostEditModel model,
             IMapper mapper,
             IBlogRepository blogRepository,
             ICategoryRepository categoryRepository,
             IAuthorRepository authorRepository,
             IMediaManager mediaManager)
        {

            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy tác giả id = {model.AuthorId}"));

            }

            if (await categoryRepository.GetCategoryByIdAsync(model.CategoryId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Không tìm thấy chủ đề id = {model.CategoryId}"));
            }

            var post = mapper.Map<Post>(model);
            post.PostedDate = DateTime.Now;

            await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post), HttpStatusCode.Created));
        }
        private static async Task<IResult> SetImagePost(int id, IFormFile file, IBlogRepository blogRepository, IMediaManager media)
        {
            var imgUrl = await media.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlPostAsync(id, imgUrl);
            return Results.Ok(ApiResponse.Success(imgUrl));
        }
        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));

            }

            var post = await blogRepository.GetPostByIdAsync(id);
            mapper.Map(model, post);
            post.Id = id;
            post.ModifiedDate = DateTime.Now;

            return await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags()) != null
                ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
        }


        private static async Task<IResult> DeletePost(
            int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Post is deleted", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Post"));

        }
        //private static async Task<IResult> GetFilter(
        //    IAuthorRepository authorRepository,
        //    IBlogRepository blogRepository)
        //{
        //    var model = new PostFilterModel()
        //    {
        //        //AuthorList;
        //    };
        //}

    }

}
