using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;
        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any()) return;
            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }
        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName="jason",
                    UrlSlug="jason-1",
                    Email="Json@gmail.com",
                    JoinedDay= new DateTime(2020,10,21)


                },
                new()
                {
                      FullName="son2",
                    UrlSlug="son-2",
                    Email="son2@gmail.com",
                    JoinedDay= new DateTime(2020,7,21)

                },
                new()
                {
                      FullName=" jas",
                    UrlSlug="jas-3",
                    Email="Json3@gmail.com",
                    JoinedDay= new DateTime(2020,4,21)

                },
                new()
                {
                      FullName=" jadson4",
                    UrlSlug="jadson-4",
                    Email="Json4@gmail.com",
                    JoinedDay= new DateTime(2020,4,21)

                },
                new()
                {
                      FullName=" jafson5",
                    UrlSlug="jafson-5",
                    Email="Json5@gmail.com",
                    JoinedDay= new DateTime(2020,1,21)

                }, new()
                {
                      FullName=" jzason6",
                    UrlSlug="jzason-6",
                    Email="Json6@gmail.com",
                    JoinedDay= new DateTime(2020,5,21)

                }
            };
            _dbContext.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() { Name = ".net core", Description = ".net", UrlSlug = "core" },
                 new() { Name = "Architecture", Description = "Architecture", UrlSlug = "Architecture" },
                  new() { Name = "Messaging", Description = "Messaging", UrlSlug = "Messaging" },
                   new() { Name = "OPP", Description = "Object_Oriented Program", UrlSlug = "Object_Oriented Program" },
                    new() { Name = "Java", Description = "JAVA", UrlSlug = "JAVA" },
                new() { Name = "python", Description = "python", UrlSlug = "python" },
                 new() { Name = "Design Patters", Description = "Design Patternse", UrlSlug = "Design Patterns" },
                  new() { Name = "CSS", Description = "Messaging CSS", UrlSlug = "Messaging CSSS" },
                   new() { Name = "HTML", Description = "Object_Oriented Program HTML", UrlSlug = "Object_Oriented Program HTML" },
                    new() { Name = "C#", Description = "Design Patterns C# ", UrlSlug = "Design Pattern C#" }
            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;

        }
        private IList<Tag> AddTags()
        {
            // http://myblog.vn/blog/post/2023/3/3/bai-hoc-hom-nya-chan-ngat

            var tags = new List<Tag>()
            {new() { Name = "Google", Description = "Google applications", UrlSlug = "google-applications" },
            new() { Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "ASP.NET MVC" },
            new() { Name = "Razor Page", Description = "Razor Page", UrlSlug = "Razor Page" },
            new() { Name = "Blazor", Description = "Blazor", UrlSlug = "Blazor" },
            new() { Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "Deep Learning" },
            new() { Name = "Neural Network", Description = "Neural Network", UrlSlug = "Neural Network" },
            new() { Name = "Go", Description = "Go applications", UrlSlug = "Go applications" },
            new() { Name = "ASPD", Description = "ASP.NNET MVC", UrlSlug = "ASP.NNET MVC" },
            new() { Name = "Raz", Description = "Razzzor Page", UrlSlug = "Razozr Page" },
            new() { Name = "Bla", Description = "Blazosr", UrlSlug = "Blazosr" },
            new() { Name = "Deep ", Description = "Deep deep Learning", UrlSlug = "Deep depp Learning" },
            new() { Name = "Neural", Description = "Neural", UrlSlug = "Neural" },
            new() { Name = "GoGo", Description = "Google po applications", UrlSlug = "Google po applications" },
            new() { Name = "MVC", Description = "ASPMVC", UrlSlug = "ASP MVC" },
            new() { Name = "Page", Description = "Page", UrlSlug = "Page" },
            new() { Name = "Bizor", Description = "Biazor", UrlSlug = "Biazor" },
            new() { Name = "Earning", Description = "Dearning", UrlSlug = "Dearning" },
            new() { Name = "work", Description = "Nwork", UrlSlug = "Ntwork" },
            new() { Name = "GoBee", Description = "GoogBee applications", UrlSlug = "Google Bee applications" },
            new() { Name = "ASC", Description = "ASC.NET MVC", UrlSlug = "ASP.NET MVC" },
            new() { Name = "Razoge", Description = "Razorge", UrlSlug = "Ra Page" },
            new() { Name = "Blaz", Description = "Blaizor", UrlSlug = "Blaizor" },
            new() { Name = "Tiktok", Description = "Deep tok Learning", UrlSlug = "Deep tok Learning" },
            new() { Name = "FB", Description = "Neu Network", UrlSlug = "Neu Network" },
            new() { Name = "YTB", Description = "Go YTB applications", UrlSlug = "Google YTB applications" },
            new() { Name = "ASTM", Description = "ASP.NEOT MVC", UrlSlug = "ASP.NEOT MVC" },
            new() { Name = "Rage", Description = "Razor Pagelo", UrlSlug = "Razor Pagelo" },
            new() { Name = "Bor", Description = "Blor", UrlSlug = "Bor" },
            new() { Name = "Learning", Description = "Deexp Learning", UrlSlug = "Deexp Learning" },
            new() { Name = "New hot", Description = "Neuriual Network", UrlSlug = "Neuriual Network" },
            new() { Name = "GoLang", Description = "Googlang", UrlSlug = "applications" },
            new() { Name = "Police", Description = "ASP.NET MVC", UrlSlug = "ASP.NET MVC" },
            new() { Name = "Computer", Description = "computer", UrlSlug = "computer" },
            new() { Name = "Phone", Description = "phone", UrlSlug = "phone" },
            new() { Name = "Trend", Description = "Trend", UrlSlug = "trend" },
            new() { Name = "Network", Description = " Network", UrlSlug = "Network" }
            };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        }
        private IList<Post> AddPosts(
               IList<Author> authors,
               IList<Category> categories,
               IList<Tag> tags
               )
        {
            var posts = new List<Post>()
                {
                    new()
                    {
                        Title = "ASP. NET core Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,10,20,0),
                        ModifiedDate = null,
                        ViewCount = 10,
                        Author = authors[0],
                        Category = categories[0],
                        Tags = new List<Tag>()
                        {
                            tags[15]
                        }

                    },
                    new()
                    {
                        Title = " Google Diagnostic Scenarios",
                        ShortDescription = "Ravid and friends has a great repos",
                        Description = "Hnere a few great DONT'T and Do examples",
                        Meta = "Diavid and friends has a great repository filled",
                        UrlSlug = "anspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,4,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[2],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[2]
                        }

                    },
                    new()
                    {
                        Title = " Python Diagnostic Scenarios",
                        ShortDescription = "Doavid and friends has a great repos",
                        Description = "Hero a few great DONT'T and Do examples",
                        Meta = "Davoid and friends has a great repository filled",
                        UrlSlug = "Raspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,2,3,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[4],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[10]
                        }

                    },new()
                    {
                        Title = " C++ Diagnostic Scenarios",
                        ShortDescription = "Bavid and friends has a great repos",
                        Description = "Pere a few great DONT'T and Do examples",
                        Meta = "Cavid and friends has a great repository filled",
                        UrlSlug = "Ospnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,5,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[4],
                        Category = categories[3],
                        Tags = new List<Tag>()
                        {
                            tags[6]
                        }

                    },new()
                    {
                        Title = " C# Diagnostic Scenarios",
                        ShortDescription = "Duavid and friends has a great repos",
                        Description = "Tere a few great DONT'T and Do examples",
                        Meta = "Duavid and friends has a great repository filled",
                        UrlSlug = "Raspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,4,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[3],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[7]
                        }

                    },new()
                    {
                        Title = " Java Diagnostic Scenarios",
                        ShortDescription = "Javid and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "Javid and friends has a great repository filled",
                        UrlSlug = "Jaspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,1,3,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[1],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[5]
                        }

                    },new()
                    {
                        Title = " GoLang Diagnostic Scenarios",
                        ShortDescription = "Gavid and friends has a great repos",
                        Description = "Gre a few great DONT'T and Do examples",
                        Meta = "Gavid and friends has a great repository filled",
                        UrlSlug = "Gaspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[1],
                        Category = categories[4],
                        Tags = new List<Tag>()
                        {
                            tags[16]
                        }

                    },new()
                    {
                        Title = " YTB Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,4,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 800,
                        Author = authors[2],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[10]
                        }

                    },new()
                    {
                        Title = " Facebook Diagnostic Scenarios",
                        ShortDescription = "Favid and friends has a great repos",
                        Description = "Fere a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "Faspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,5,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 190,
                        Author = authors[5],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[13]
                        }

                    },new()
                    {
                        Title = " Police Diagnostic Scenarios",
                        ShortDescription = "Pavid and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,8,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 100,
                        Author = authors[4],
                        Category = categories[4],
                        Tags = new List<Tag>()
                        {
                            tags[10]
                        }

                    },new()
                    {
                        Title = " Hello Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,4,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 130,
                        Author = authors[1],
                        Category = categories[1],
                        Tags = new List<Tag>()
                        {
                            tags[20]
                        }

                    },new()
                    {
                        Title = " Hiiiiii Diagnostic Scenarios",
                        ShortDescription = "Lavid and friends has a great repos",
                        Description = "Lere a few great DONT'T and Do examples",
                        Meta = "Lavid and friends has a great repository filled",
                        UrlSlug = "Aospnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,8,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 10,
                        Author = authors[3],
                        Category = categories[1],
                        Tags = new List<Tag>()
                        {
                            tags[5]
                        }

                    },new()
                    {
                        Title = " Go to school Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 150,
                        Author = authors[5],
                        Category = categories[4],
                        Tags = new List<Tag>()
                        {
                            tags[16]
                        }

                    },new()
                    {
                        Title = " Linux Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,11,30,11,20,0),
                        ModifiedDate = null,
                        ViewCount = 500,
                        Author = authors[2],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[19]
                        }

                    },new()
                    {
                        Title = " Window Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 300,
                        Author = authors[4],
                        Category = categories[5],
                        Tags = new List<Tag>()
                        {
                            tags[14]
                        }

                    },new()
                    {
                        Title = " Laptop Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 1000,
                        Author = authors[3],
                        Category = categories[8],
                        Tags = new List<Tag>()
                        {
                            tags[10]
                        }

                    },new()
                    {
                        Title = " Phone Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,7,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 160,
                        Author = authors[4],
                        Category = categories[6],
                        Tags = new List<Tag>()
                        {
                            tags[5]
                        }

                    },new()
                    {
                        Title = " GoPaPa Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,6,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 1200,
                        Author = authors[2],
                        Category = categories[7],
                        Tags = new List<Tag>()
                        {
                            tags[9]
                        }

                    },new()
                    {
                        Title = " Ge Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "Da friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,5,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 1800,
                        Author = authors[0],
                        Category = categories[7],
                        Tags = new List<Tag>()
                        {
                            tags[8]
                        }

                    },new()
                    {
                        Title = " Giagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,4,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 1700,
                        Author = authors[2],
                        Category = categories[8],
                        Tags = new List<Tag>()
                        {
                            tags[18]
                        }

                    },new()
                    {
                        Title = " Tiktok Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,3,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 1020,
                        Author = authors[5],
                        Category = categories[2],
                        Tags = new List<Tag>()
                        {
                            tags[13]
                        }

                    },new()
                    {
                        Title = " Web Diagnostic Scenarios",
                        ShortDescription = "David and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "David and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,2,4,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 400,
                        Author = authors[0],
                        Category = categories[4],
                        Tags = new List<Tag>()
                        {
                            tags[7]
                        }

                    },new()
                    {
                        Title = " Dlu Diagnostic Scenarios",
                        ShortDescription = "Dlu and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "Dlu and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,9,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 500,
                        Author = authors[3],
                        Category = categories[8],
                        Tags = new List<Tag>()
                        {
                            tags[11]
                        }

                    },new()
                    {
                        Title = "Python",
                        ShortDescription = "Proggram and friends has a great repos",
                        Description = "Here a few great DONT'T and Do examples",
                        Meta = "Python and friends has a great repository filled",
                        UrlSlug = "aspnet-core-diagnostic-scenarios",
                        Published = true,
                        PostedDate = new DateTime(2021,1,30,12,20,0),
                        ModifiedDate = null,
                        ViewCount = 700,
                        Author = authors[3],
                        Category = categories[9],
                        Tags = new List<Tag>()
                        {
                            tags[12]
                        }

                    }
                };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }

    }
}


