using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using Microsoft.EntityFrameworkCore;
using TatBlog.WebApp.Extension;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.Validations;
//using TatBlog.Services.Blogs;

//var builder = WebApplication.CreateBuilder(args);
//{
//    builder.Services.AddControllersWithViews();
//    //    builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(
//    //        builder.Configuration.GetConnectionString("DefaultConnection")));
//    //    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
//    //    builder.Services.AddScoped<IDataSeeder, IDataSeeder>();
//}
//var app = builder.Build();
//{
//    if (app.Environment.IsDevelopment())
//    {
//        app.UseDeveloperExceptionPage();
//    }
//    else
//    {
//        app.UseExceptionHandler("/Blog/Error");
//        app.UseHsts();
//    }
//    app.UseHttpsRedirection();
//    app.UseStaticFiles();
//    app.UseRouting();
//    //app.MapControllerRoute(
//    //   name: "posts-by-category",
//    //   pattern: "blog/category/{slug}",
//    //   defaults:new {controller="Blog",action="Category"});

//    //app.MapControllerRoute(
//    //   name: "posts-by-tag",
//    //   pattern: "blog/tag/{slug}",
//    //   defaults: new { controller = "Blog", action = "Tag" });

//    //app.MapControllerRoute(
//    //   name: "posts-by-category",
//    //   pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
//    //   defaults: new { controller = "Blog", action = "Post" });




//    app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Blog}/{action=Index}/{id?}");

//}
//using(var scope =app.Services.CreateScope())
//{
//    //var seeder=scope.ServiceProvider.GetRequiredService<IDataSeeder>();
//    seeder.Initialize();
//}
//
//Test

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .ConfigureMvc()
        .ConfigureServices()
        .ConfigureFluentValidation()
        .ConfigureNLog()
        .ConfigureMapster();

}
var app = builder.Build();
{
    app.UseDataSeeder();
    app.UseBlogRoutes();
    app.UseRequestPipeline();
}
app.Run();

