using Azure;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using System.Collections.Immutable;
using TatBlog.WinApp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Data.SqlClient;

//var context = new BlogDbContext();
//var seeder = new DataSeeder(context);
//seeder.Initialize();
//var authors = context.Authors.ToList();
//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
//    "ID", "Full Name", "Email", "Joined Dated");
//foreach (var author in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDay);
//}

//Lấy kèm tên tác giả và chuyên mục
//var context = new BlogDbContext();
//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCout = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullName,
//        Category = p.Category.Name,
//    }
//    )
//    .ToList();
//Xuất danh sách
//foreach (var post in posts)
//{
//    Console.WriteLine("ID:{0}", post.Id);
//    Console.WriteLine("Title:{0}", post.Title);
//    Console.WriteLine("View:{0}", post.ViewCout);
//    Console.WriteLine("Date:{0:MM//dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author:{0}", post.Author);
//    Console.WriteLine("Category:{0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}

// 3 bài viết xem nhiều nhất
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var posts = await blogRepo.GetPoPularArticlesAsysc(3);
//foreach (var post in posts)
//{
//    Console.WriteLine("ID:{0}", post.Id);
//    Console.WriteLine("Title:{0}", post.Title);
//    Console.WriteLine("View:{0}", post.ViewCount);
//    Console.WriteLine("Date:{0:MM//dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author:{0}", post.Author.FullName);
//    Console.WriteLine("Category:{0}", post.Category.Name);
//    Console.WriteLine("".PadRight(80, '-'));
//}

//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var categories = await blogRepo.GetCategoriesAsyns();
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
//foreach (var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);

//}
// text
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Name",
//    SortOrder = "DESC"
//};
//var tagList = await blogRepo.GetPagedTagsAsync(pagingParams);
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
//foreach (var item in tagList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);

//}

//Tìm thẻ tag bằng slug
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var tags = await blogRepo.GetTagAsync("Deep Learning");
//Console.WriteLine("{0,-5}{1,-50}", tags.Id, tags.Name,tags.Description);

//Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var tags = await blogRepo.GetTagesAsyns();
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
//foreach (var item in tags)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);

//Tìm một bài viết theo mã sốA'
var context = new BlogDbContext();
IBlogRepository blogRepo = new BlogRepository(context);
//var post = await blogRepo.FindPostByIdAsync(9);
//Console.WriteLine("{0, -10}{1, -50}{2, -50}",
//    post.Id, post.Title, post.ShortDescription);

//var query = new TatBlog.Core.DTO.PostQuery()
//{
//    AuthorId = 2,
//    CategoryId = 2,
//    SlugCategory = "Messaging",
//    TimeCreated = DateTime.Parse("2022-11-08"),
//    Tag = "Razor Page"
//};

//var posts = await blogRepo.FindPostByPostQueryAsync(query);
//int count = 1;

//foreach (var post in posts)
//{
//    Console.WriteLine(count++);
//    Console.WriteLine("---------------------------------------------------\n");
//    Console.WriteLine("Author ID: " + post.AuthorId);
//    Console.WriteLine("Category ID: " + post.CategoryId);
//    Console.WriteLine("Category Slug: " + post.Category.UrlSlug);
//    Console.WriteLine("Month: " + post.PostedDate.Month);
//    Console.WriteLine("Year: " + post.PostedDate.Year);
//}


//var paringParams = new PagingParams()
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "ViewCount",
//    SortOrder = "DESC"
//};
//var query = new TatBlog.Core.DTO.PostQuery()
//{
//    AuthorId = 2,
//    CategoryId = 2,

//    SlugCategory = "Messaging",
//    TimeCreated = DateTime.Parse("2022-11-08"),
//    Tag = "Razor Page",
//};

//var posts = await blogRepo.GetPageFindPostByPostQueryAsync(paringParams, query);
//foreach (var post in posts)
//{
//    Console.WriteLine(post);
//}

var paringParams = new PagingParams()
{
    PageNumber = 1,
PageSize = 10,
SortColumn = "ViewCount",
    SortOrder = "DESC"
};
var condition = new TatBlog.Core.DTO.PostQuery()
{
    AuthorId = 2,
    CategoryId = 2,
    SlugCategory = "Messaging",
    Year = 2022,
    Month = 2,
    PostedDate = DateTime.Parse("2022-11-08"),
    Tag = "Razor Page",
};

var posts = await blogRepo.GetPagePostsAsync(condition, 1, 10);
foreach (var post in posts)
{
    Console.WriteLine(post);
}

// var newPost = new Category()
//{
//    Name = "Florua",
//    Description = "information florua",
//    UrlSlug = "Florua",
//};
//var Change = await blogRepo.AddOrEditCategoryAsync(newPost);
//Console.WriteLine(Change ? "Update success" : "Failed, try again");
