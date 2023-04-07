using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApi.Models
{
<<<<<<< Updated upstream:src/TipsAndTricks/TatBlog.WebApi/Models/AuhorFilterModel.cs
    public class AuhorFilterModel:PagingModel
    {
        public string Name { get; set; }
=======
    public class PostFilterModel : PagingModel
    {


        //public string Keyword { get; set; }
        //public int? AuthorId { get; set; }
        //public int? CategoryId { get; set; }
        public bool? Published { get; set; }
        //public string CategorySlug { get; set; }
        //public string TitleSlug { get; set; }
        //public string TagSlug { get; set; }
        //public string AuthorSlug { get; set; }
        //public int? Year { get; set; }
        //public int? Month { get; set; }
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
        [DisplayName("Tác giả")]
        public int? AuthorId { get; set; }
        [DisplayName("Chủ đề")]
        public int? CategoryId { get; set; }
        [DisplayName("Năm")]
        public int? Year { get; set; }
        [DisplayName("Tháng")]
        public int? Month { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public PostFilterModel()
        {
            MonthList = Enumerable.Range(1, 12)
            .Select(m => new SelectListItem()
            {
                Value = m.ToString(),
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
            })
           .ToList();
        }
>>>>>>> Stashed changes:src/TipsAndTricks/TatBlog.WebApi/Models/PostFilterModel.cs
    }

}

