using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PostsBlog.Services.Blogs;
using System.Net;
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
            var routeGroupBuilder = app.MapGroup("/api/Posts");
            routeGroupBuilder.MapGet("/", GetPosts)
                .WithName("GetPosts")
                .Produces<ApiResponse<PaginationResult<PostItem>>>();
            //.Produces<PaginationResult<PostItem>>();



            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostItem>>();
            //.Produces<PostItem>()
            //.Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9 -]+$)}/Post", GetPostByPostId)
               .WithName("GetPostByPostSlug")
               .Produces<ApiResponse<PaginationResult<PostDto>>>();
            //.Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<PostItem>>();
            //.Produces(201)
            //.Produces(400)
            //.Produces<ApiResponse<string>>();
            //.Produces(409);

            //routeGroupBuilder.MapPost("/{id:int}/avtar", SetPostPicture)
            //    .WithName("SetPostPicture")
            //    .Accepts<IFormFile>("mutipart/form-data")
            //    .Produces(401)
            //    .RequireAuthorization()
            //    .Produces<ApiResponse<string>>();
            //.Produces<string>
            //.Produces(400);

            routeGroupBuilder.MapPost("/{id:int}", UpdatePost)
                .WithName("UpdateAnPost")
                .RequireAuthorization()
                .Produces(401);
            //.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
            //.Produces(204)
            //.Produces(400)
            //.Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<string>>();
            //.Produces(204)
            //.Produces(404);



            return app;
        }
        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model, IPostRepository PostRepository)
        {
            var PostsList = await PostRepository
                .GetPagedPostsAsync(model, model.Name);
            var paginationResult = new PaginationResult<PostItem>(PostsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        public static async Task<IResult> GetPostDetails(
            int id, IPostRepository PostRepository, IMapper mapper)
        {
            var Post = await PostRepository.GetCachedPostByIdAsync(id);
            return Post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(Post)));
        }
        public static async Task<IResult> GetPostByPostId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
               PostId  = id,
                PublishedOnly = true
            };
            var PostList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                Post => Post.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(PostList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        public static async Task<IResult> GetPostByPostSlug(
             [FromRoute] string Slug,
             [AsParameters] PagingModel pagingModel,
             IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
               TitleSlug = Slug,
                PublishedOnly = true
            };
            var PostList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                Post => Post.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(PostList);
            return Results.Ok(paginationResult);
        }
        //private static async Task<IResult> AddPost(
        //    PostEditModel model,
        //    IValidator<PostEditModel> validator,
        //    IPostRepository PostRepository,
        //    IMapper mapper)
        //{
        //    var validationResult=await validator.ValidateAsync(model);
        //    if(!validationResult.IsValid)
        //    {
        //        return Results.BadRequest(validationResult.Errors.ToResponse());

        //    }
        //    if (await PostRepository.IsPostSlugExistedAsync(0,model.UrlSlug))
        //    {
        //        return Results.Conflict(
        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
        //    }
        //    var Post = mapper.Map<Post>(model);
        //    await PostRepository.AddOrUpdateAsync(Post);
        //    return Results.CreatedAtRoute("GetPostById", new {Post.Id},
        //        mapper.Map<PostItem>(Post));
        //}
        public static async Task<IResult> AddPost(
            PostEditModel model, IPostRepository PostRepository, IMapper mapper)
        {
            if (await PostRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"slug'{model.UrlSlug}' Đã được sử dụng"));
            }
            var Post = mapper.Map<Post>(model);
            await PostRepository.AddOrUpdateAsync(Post);
            return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(Post), HttpStatusCode.Created));
            ;
        }
        //private static async Task<IResult> SetPostPicture(
        //    int id, IFormFile imageFile,
        //    IPostRepository PostRepository,
        //    IMediaManager mediaManager)
        //{
        //    var imageUrl = await mediaManager.SaveFileAsync(
        //        imageFile.OpenReadStream(),
        //        imageFile.FileName, imageFile.ContentType);

        //    if (string.IsNullOrWhiteSpace(imageUrl))
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
        //    }
        //    await PostRepository.SetImageUrlAsync(id, imageUrl);
        //    return Results.Ok(ApiResponse.Success(imageUrl));

        //}
        //private static async Task<IResult> UpdatePost(
        //    int id,
        //    PostEditModel model,
        //    IValidator<PostEditModel> validator,
        //    IPostRepository PostRepository,
        //    IMapper mapper)
        //{
        //    var validationResult = await validator.ValidateAsync(model);
        //    if (!validationResult.IsValid)
        //    {
        //        return Results.BadRequest(validationResult.Errors.ToResponse());

        //    }
        //    if (await PostRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
        //    {
        //        return Results.Conflict(
        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
        //    }
        //    var Post = mapper.Map<Post>(model);
        //    Post.Id= id;
        //    return await PostRepository.AddOrUpdateAsync(Post)
        //        ? Results.NoContent()
        //        : Results.NotFound();
        //}
        private static async Task<IResult> UpdatePost(
            int id,
            PostEditModel model,
            IValidator<PostEditModel> validator,
            IPostRepository PostRepository,
            IMapper mapper)
        {

            var validationResult = await validator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
            }
            if (await PostRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            //if (await PostRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            //{
            //    return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest
            //        $"Slug '{model.UrlSlug}' đã được sử dụng");
            //}
            var Post = mapper.Map<Post>(model);
            Post.Id = id;
            return await PostRepository.AddOrUpdateAsync(Post)
                ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Post"));
        }
        private static async Task<IResult> DeletePost(
            int id, IPostRepository PostRepository)
        {
            return await PostRepository.DeletePostAsync(id)
                ? Results.Ok(ApiResponse.Success("Post is deleted", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Post"));

        }

    }

}
