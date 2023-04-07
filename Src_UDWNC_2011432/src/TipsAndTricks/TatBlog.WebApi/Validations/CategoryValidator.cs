using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryEditModel>
    {
        public CategoryValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Tên tác giảkhông được để trống")
                .MaximumLength(100)
                .WithMessage("Tên tác giả tối đa 100 kí tự");
            RuleFor(a => a.UrlSlug)
               .NotEmpty()
               .WithMessage("UrlSlug không được để trống")
               .MaximumLength(100)
                .WithMessage("UrlSlug tối đa 100 kí tự");
            RuleFor(a => a.Description)
               .NotEmpty()
               .WithMessage("Des không được để trống")
               .MaximumLength(1000)
                .WithMessage("UrlSlug tối đa 1000 kí tự");
           
            
              

           

        }
    }
}
