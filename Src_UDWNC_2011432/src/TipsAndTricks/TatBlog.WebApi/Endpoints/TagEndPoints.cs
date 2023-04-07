//using FluentValidation;
//using MapsterMapper;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using TagBlog.Services.Media;
//using TatBlog.Core.Collections;
//using TatBlog.Core.DTO;
//using TatBlog.Core.Entities;
//using TatBlog.Services.Blogs;
//using TatBlog.WebApi.Filters;
//using TatBlog.WebApi.Models;

//namespace TatBlog.WebApi.Endpoints
//{
//    public static class TagEndpoints
//    {
//        //public static WebApplication MapTagEndpoints(this WebApplication app)
//        //{
//        //    var routeGroupBuilder = app.MapGroup("/api/Tags");
//        //    routeGroupBuilder.MapGet("/", GetTags)
//        //        .WithName("GetTags")
//        //        .Produces<PaginationResult<TagItem>>();
//        //    return app;
//        //}
//        public static WebApplication MapTagEndpoints(this WebApplication app)
//        {
//            var routeGroupBuilder = app.MapGroup("/api/Tags");
//            routeGroupBuilder.MapGet("/", GetTags)
//                .WithName("GetTags")
//                .Produces<ApiResponse<PaginationResult<TagItem>>>();
//            //.Produces<PaginationResult<TagItem>>();



//            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
//                .WithName("GetTagById")
//                .Produces<ApiResponse<TagItem>>();
//            //.Produces<TagItem>()
//            //.Produces(404);

//            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9 -]+$)}/posts", GetPostsByTagId)
//               .WithName("GetPostsByTagSlug")
//               .Produces<ApiResponse<PaginationResult<PostDto>>>();
//            //.Produces<PaginationResult<PostDto>>();

//            routeGroupBuilder.MapPost("/", AddTag)
//                .WithName("AddNewTag")
//                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
//                .RequireAuthorization()
//                .Produces(401)
//                .Produces<ApiResponse<TagItem>>();
//            //.Produces(201)
//            //.Produces(400)
//            //.Produces<ApiResponse<string>>();
//            //.Produces(409);

           

//            routeGroupBuilder.MapPost("/{id:int}", UpdateTag)
//                .WithName("UpdateAnTag")
//                .RequireAuthorization()
//                .Produces(401);
//            //.AddEndpointFilter<ValidatorFilter<TagEditModel>>()
//            //.Produces(204)
//            //.Produces(400)
//            //.Produces(409);

//            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
//                .WithName("DeleteTag")
//                .RequireAuthorization()
//                .Produces(401)
//                .Produces<ApiResponse<string>>();
//            //.Produces(204)
//            //.Produces(404);



//            return app;
//        }
//        private static async Task<IResult> GetTags(
//            [AsParameters] TagFilterModel model, ITagRepository TagRepository)
//        {
//            var TagsList = await TagRepository
//                .GetPagedTagsAsync(model, model.Name);
//            var paginationResult = new PaginationResult<TagItem>(TagsList);
//            return Results.Ok(ApiResponse.Success(paginationResult));
//        }
//        public static async Task<IResult> GetTagDetails(
//            int id, ITagRepository TagRepository, IMapper mapper)
//        {
//            var Tag = await TagRepository.GetCachedTagByIdAsync(id);
//            return Tag == null
//                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
//                : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(Tag)));
//        }
//        public static async Task<IResult> GetPostsByTagId(
//            int id,
//            [AsParameters] PagingModel pagingModel,
//            IBlogRepository blogRepository)
//        {
//            var postQuery = new PostQuery()
//            {
//                TagId = id,
//                PublishedOnly = true
//            };
//            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
//                posts => posts.ProjectToType<PostDto>());
//            var paginationResult = new PaginationResult<PostDto>(postsList);
//            return Results.Ok(ApiResponse.Success(paginationResult));
//        }
//        public static async Task<IResult> GetPostsByTagSlug(
//             [FromRoute] string Slug,
//             [AsParameters] PagingModel pagingModel,
//             IBlogRepository blogRepository)
//        {
//            var postQuery = new PostQuery()
//            {
//                TagSlug = Slug,
//                PublishedOnly = true
//            };
//            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
//                posts => posts.ProjectToType<PostDto>());
//            var paginationResult = new PaginationResult<PostDto>(postsList);
//            return Results.Ok(paginationResult);
//        }
//        //private static async Task<IResult> AddTag(
//        //    TagEditModel model,
//        //    IValidator<TagEditModel> validator,
//        //    ITagRepository TagRepository,
//        //    IMapper mapper)
//        //{
//        //    var validationResult=await validator.ValidateAsync(model);
//        //    if(!validationResult.IsValid)
//        //    {
//        //        return Results.BadRequest(validationResult.Errors.ToResponse());

//        //    }
//        //    if (await TagRepository.IsTagSlugExistedAsync(0,model.UrlSlug))
//        //    {
//        //        return Results.Conflict(
//        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
//        //    }
//        //    var Tag = mapper.Map<Tag>(model);
//        //    await TagRepository.AddOrUpdateAsync(Tag);
//        //    return Results.CreatedAtRoute("GetTagById", new {Tag.Id},
//        //        mapper.Map<TagItem>(Tag));
//        //}
//        public static async Task<IResult> AddTag(
//            TagEditModel model, ITagRepository TagRepository, IMapper mapper)
//        {
//            if (await TagRepository.IsTagSlugExistedAsync(0, model.UrlSlug))
//            {
//                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"slug'{model.UrlSlug}' Đã được sử dụng"));
//            }
//            var Tag = mapper.Map<Tag>(model);
//            await TagRepository.AddOrUpdateAsync(Tag);
//            return Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(Tag), HttpStatusCode.Created));
//            ;
//        }
//        private static async Task<IResult> SetTagPicture(
//            int id, IFormFile imageFile,
//            ITagRepository TagRepository,
//            IMediaManager mediaManager)
//        {
//            var imageUrl = await mediaManager.SaveFileAsync(
//                imageFile.OpenReadStream(),
//                imageFile.FileName, imageFile.ContentType);

//            if (string.IsNullOrWhiteSpace(imageUrl))
//            {
//                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
//            }
//            await TagRepository.SetImageUrlAsync(id, imageUrl);
//            return Results.Ok(ApiResponse.Success(imageUrl));

//        }
//        //private static async Task<IResult> UpdateTag(
//        //    int id,
//        //    TagEditModel model,
//        //    IValidator<TagEditModel> validator,
//        //    ITagRepository TagRepository,
//        //    IMapper mapper)
//        //{
//        //    var validationResult = await validator.ValidateAsync(model);
//        //    if (!validationResult.IsValid)
//        //    {
//        //        return Results.BadRequest(validationResult.Errors.ToResponse());

//        //    }
//        //    if (await TagRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
//        //    {
//        //        return Results.Conflict(
//        //            $"Slug '{model.UrlSlug}' đã được sử dụng");
//        //    }
//        //    var Tag = mapper.Map<Tag>(model);
//        //    Tag.Id= id;
//        //    return await TagRepository.AddOrUpdateAsync(Tag)
//        //        ? Results.NoContent()
//        //        : Results.NotFound();
//        //}
//        private static async Task<IResult> UpdateTag(
//            int id,
//            TagEditModel model,
//            IValidator<TagEditModel> validator,
//            ITagRepository TagRepository,
//            IMapper mapper)
//        {

//            var validationResult = await validator.ValidateAsync(model);

//            if (validationResult.IsValid)
//            {
//                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
//            }
//            if (await TagRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
//            {
//                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
//                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
//            }
//            //if (await TagRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
//            //{
//            //    return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest
//            //        $"Slug '{model.UrlSlug}' đã được sử dụng");
//            //}
//            var Tag = mapper.Map<Tag>(model);
//            Tag.Id = id;
//            return await TagRepository.AddOrUpdateAsync(Tag)
//                ? Results.Ok(ApiResponse.Success("Tag is updated", HttpStatusCode.NoContent))
//                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Tag"));
//        }
//        private static async Task<IResult> DeleteTag(
//            int id, ITagRepository TagRepository)
//        {
//            return await TagRepository.DeleteTagAsync(id)
//                ? Results.Ok(ApiResponse.Success("Tag is deleted", HttpStatusCode.NoContent))
//                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could find Tag"));

//        }

//    }
//}
//}
