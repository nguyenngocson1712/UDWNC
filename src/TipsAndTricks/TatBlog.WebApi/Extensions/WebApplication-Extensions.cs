﻿using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TagBlog.Services.Blogs;
using TagBlog.Services.Media;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Timing;

namespace TatBlog.WebApi.Extensions
{
    public static class WebApplication_Extensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITimeProvider,LocalTimeProvider>();
            builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped <IBlogRepository, BlogRepository>();
            builder.Services.AddScoped <IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            return builder;
        }
        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("TatBlogApp", policyBuilder => policyBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });
            return builder;
        }
         public static WebApplicationBuilder ConfigureNlog(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }
        public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            return builder;
        }
        public static WebApplication SetupReQuestPipeline( this WebApplication app)
        {
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("TatBlogApp");
            return app;
        }
    }
}
