using System.Data.SqlTypes;
namespace Tiny.Api.Extenstions;

internal static class StartupExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(mvcOptions => mvcOptions.SetResultConvention());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAssemblyServices();

        return builder;
    }

    public static WebApplication ConfigureServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static IServiceCollection AddAssemblyServices(this IServiceCollection services)
    {
        Application.ConfigureServiceContainer.AddServices(services);
        Infrastructure.ConfigureServiceContainer.AddServices(services);
        Api.ConfigureServiceContainer.AddServices(services);

        return services;
    }
}
