using TatBlog.WebApi.Extensions;
var builder = WebApplication.CreateBuilder(args);

{

    builder.ConfigureCors()
       .ConfigureNlog()
       .ConfigureServices()
       .ConfigureSwaggerOpenApi();
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
app.SetupReQuestPipeline();
app.Run();


