using Tiny.Api.Extenstions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.AddServices().Build();
app.ConfigureServices().Run();
