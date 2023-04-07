using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);

{

    builder.ConfigureCors()
       .ConfigureNlog()
       .ConfigureServices()
       .ConfigureSwaggerOpenApi()
       .ConfigureMapster()
       .ConfigureFluentValidaton();
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
app.SetupReQuestPipeline();
app.MapAuthorEndpoints();
app.MapCategoryEndpoints();
app.MapPostEndpoints();
app.Run();


