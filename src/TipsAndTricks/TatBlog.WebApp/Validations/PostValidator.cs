using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public PostValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage ("Tiêu đề không được để trống")
                .MaximumLength(500);
            RuleFor(x => x.ShortDescription)
               .NotEmpty()
               .WithMessage("Giới thiệu không được để trống");
            RuleFor(x => x.Description)
               .NotEmpty()
                .WithMessage("Nội dung không được để trống"); ;
            RuleFor(x => x.Meta)
            .NotEmpty()
             .WithMessage("Meta không được để trống")
            .MaximumLength(1000);
            RuleFor(x => x.UrlSlug)
               .NotEmpty()
               .MaximumLength(1000);
            RuleFor(x => x.UrlSlug)
                .MustAsync(async (postModel, slug, CancellationToken) =>
                !await blogRepository.IsPostSlugExistedAsync(
                    postModel.Id, slug, CancellationToken))
                .WithMessage("Slug'{PropertyValue}'đã được sử dụng");
            RuleFor(x => x.CategoryId)
              .NotEmpty()
              .WithMessage("Bạn phải chọn chủ đề bài viết");
            RuleFor(x => x.AuthorId)
              .NotEmpty()
              .WithMessage("Bạn phải chọn tác giả bài viết");
            RuleFor(x => x.SelectedTags)
              .Must(HasAtLeastOneTag)
            .WithMessage("Bạn phải nhập ít nhất 1 thẻ");
            When(x => x.Id <= 0, () =>
             {
                 RuleFor(x => x.ImageFile)
                 .Must(x => x is { Length: > 0 })
                 .WithMessage("Bạn phải chọn hình ảnh");
             })
                .Otherwise(() =>
                {
                    RuleFor(x => x.ImageFile)
                  .MustAsync(SetImageIfNotExist)
                  .WithMessage("Bạn phải chọn hình ảnh");
                });
        }
        private bool HasAtLeastOneTag(
            PostEditModel postModel, string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }
        private async Task<bool> SetImageIfNotExist(
            PostEditModel postModel, IFormFile imageFile, CancellationToken cancellationToken )
        {
            var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;
            return imageFile is { Length: > 0 };
        }



    }
}
