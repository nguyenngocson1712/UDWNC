using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PostId { get; set; }
        public string SlugCategory { get; set; }

        public string AuthorSlug { get; set; }

        public string TagSlug { get; set; }

        public string TitleSlug { get; set; }

        public string Keyword { get; set; }
        public DateTime TimeCreated { get; set; }

        public DateTime PostedDate{ get; set; }
         public int PostedYear { get; set; }

        public int PostedMonth { get; set; }

        public int ViewCount { get; set; }
        public string Tag { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool PublishedOnly { get; set; }
        public bool NotPublished { get; set; }
    }

}
