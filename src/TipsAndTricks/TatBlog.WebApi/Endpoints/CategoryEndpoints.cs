using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    public static class CategoryEndpoints
    {
        //public static WebApplication MapCategoryEndpoints(this WebApplication app)
        //{
        //    var routeGroupBuilder = app.MapGroup("/api/Categorys");
        //    routeGroupBuilder.MapGet("/", GetCategorys)
        //        .WithName("GetCategorys")
        //        .Produces<PaginationResult<CategoryItem>>();
        //    return app;
        //}
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");
            routeGroupBuilder.MapGet("/",GetCategorys)
                .WithName("GetCategories")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();
            //.Produces<PaginationResult<CategoryItem>>();



            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();
            //.Produces<CategoryItem>()
            //.Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9 -]+$)}/posts", GetPostsByCategoryId)
               .WithName("GetPostsByCategorySlug")
               .Produces<ApiResponse<PaginationResult<PostDto>>>();
            //.Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<CategoryItem>>();
            //.Produces(201)
            //.Produces(400)
            //.Produces<ApiResponse<string>>();
            //.Produces(409);

            

            routeGroupBuilder.MapPost("/{id:int}", UpdateCategory)
                .WithName("UpdateAnCategory")
                .RequireAuthorization()
                .Produces(401);
            //.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
            //.Produces(204)
            //.Produces(400)
            //.Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteCategory")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<string>>();
            //.Produces(204)
            //.Produces(404);



            return app;
        }
        private static async Task<IResult> GetCategorys(
           [AsParameters] CategoryFilterModel model, ICategoryRepository categoryRepository)
        {
            var categorysList = await categoryRepository
                .GetPagedCategorysAsync(model, model.Name);
            var paginationResult = new PaginationResult<CategoryItem>(categorysList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        
        public static async Task<IResult> GetCategoryDetails(
            int id, ICategoryRepository CategoryRepository, IMapper mapper)
        {
            var Category = await CategoryRepository.GetCachedCategoryByIdAsync(id);
            return Category == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(Category)));
        }
        public static async Task<IResult> GetPostsByCategoryId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategoryId = id,
                PublishedOnly = true
            };
            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        public static async Task<IResult> GetPostsByCategorySlug(
             [FromRoute] string Slug,
             [AsParameters] PagingModel pagingModel,
             IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                SlugCategory = Slug,
                PublishedOnly = true
            };
            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }
        //private static async Task<IResult> AddCategory(
        //    CategoryEditModel model,
        //    IValidator<CategoryEditModel> validator,
        //    ICategoryRepository CategoryRepository,
        //    IMapper mapper)
        //{
        //    var validationResult=await validator.ValidateAsync(model);
        //    if(!validationResult.IsValid)
        //    {
        //        return Results.BadRequest(validationResult.Errors.ToResponse());

        //    }
        //    if (await CategoryRepository.IsCategorySlugExistedAsync(0,model.UrlSlug))
        //    {
        //        return Results.Conflict(
        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
        //    }
        //    var Category = mapper.Map<Category>(model);
        //    await CategoryRepository.AddOrUpdateAsync(Category);
        //    return Results.CreatedAtRoute("GetCategoryById", new {Category.Id},
        //        mapper.Map<CategoryItem>(Category));
        //}
        public static async Task<IResult> AddCategory(
            CategoryEditModel model, ICategoryRepository CategoryRepository, IMapper mapper)
        {
            if (await CategoryRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"slug'{model.UrlSlug}' Đã được sử dụng"));
            }
            var Category = mapper.Map<Category>(model);
            await CategoryRepository.AddOrUpdateAsync(Category);
            return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(Category), HttpStatusCode.Created));
            ;
        }
        //private static async Task<IResult> SetCategoryPicture(
        //    int id, IFormFile imageFile,
        //    ICategoryRepository CategoryRepository,
        //    IMediaManager mediaManager)
        //{
        //    var imageUrl = await mediaManager.SaveFileAsync(
        //        imageFile.OpenReadStream(),
        //        imageFile.FileName, imageFile.ContentType);

        //    if (string.IsNullOrWhiteSpace(imageUrl))
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
        //    }
        //    await CategoryRepository.SetImageUrlAsync(id, imageUrl);
        //    return Results.Ok(ApiResponse.Success(imageUrl));

        //}
        //private static async Task<IResult> UpdateCategory(
        //    int id,
        //    CategoryEditModel model,
        //    IValidator<CategoryEditModel> validator,
        //    ICategoryRepository CategoryRepository,
        //    IMapper mapper)
        //{
        //    var validationResult = await validator.ValidateAsync(model);
        //    if (!validationResult.IsValid)
        //    {
        //        return Results.BadRequest(validationResult.Errors.ToResponse());

        //    }
        //    if (await CategoryRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
        //    {
        //        return Results.Conflict(
        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
        //    }
        //    var Category = mapper.Map<Category>(model);
        //    Category.Id= id;
        //    return await CategoryRepository.AddOrUpdateAsync(Category)
        //        ? Results.NoContent()
        //        : Results.NotFound();
        //}
        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IValidator<CategoryEditModel> validator,
            ICategoryRepository CategoryRepository,
            IMapper mapper)
        {

            var validationResult = await validator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
            }
            if (await CategoryRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            //if (await CategoryRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
            //{
            //    return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest
            //        $"Slug '{model.UrlSlug}' đã được sử dụng");
            //}
            var Category = mapper.Map<Category>(model);
            Category.Id = id;
            return await CategoryRepository.AddOrUpdateAsync(Category)
                ? Results.Ok(ApiResponse.Success("Category is updated", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Category"));
        }
        private static async Task<IResult> DeleteCategory(
            int id, ICategoryRepository CategoryRepository)
        {
            return await CategoryRepository.DeleteCategoryAsync(id)
                ? Results.Ok(ApiResponse.Success("Category is deleted", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Category"));

        }

    }
}

