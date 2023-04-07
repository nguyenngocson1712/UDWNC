using Mapster;
<<<<<<< Updated upstream
=======
using TagBlog.Services.Blogs;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models;
>>>>>>> Stashed changes

namespace TatBlog.WebApi.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
<<<<<<< Updated upstream
            throw new NotImplementedException();
=======
            config.NewConfig<Author,AuthorDto>();
            config.NewConfig<Author, AuthorItem>().Map(dest => dest.PostCount,
                src => src.Posts==null? 0 : src.Posts.Count);
            config.NewConfig<AuthorEditModel, Author>();
            config.NewConfig<Category, CategoryDto>();
            config.NewConfig<Category, CategoryItem>().Map(dest => dest.PostCount,
                src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<PostFilterModel, PostQuery>()
               .Map(dest => dest.PublishedOnly, src => src.Published == true ? true : false)
               .Map(dest => dest.NotPublished, src => src.Published != true ? true : false);
            config.NewConfig<Post, PostDto>();
            config.NewConfig<PostEditModel, Post>()
               .Ignore(dest => dest.ImageUrl);
            config.NewConfig<Post,PostDetail>();
>>>>>>> Stashed changes
        }
    }
}
