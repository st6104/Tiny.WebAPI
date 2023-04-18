using Swashbuckle.AspNetCore.SwaggerGen;
using Tiny.Api.Conventions;
using Tiny.Api.Middlewares;
using Tiny.Api.OperationFilters;
using Tiny.MultiTenant.Extensions;

namespace Tiny.Api.Extenstions;

internal static class StartupExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(mvcOptions => mvcOptions.Conventions.Add(new TinyApiConvention()));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
        {
            config.SupportNonNullableReferenceTypes();
            config.OperationFilter<TenantHeaderSwaggerAttribute>();
            config.AddSwaggerXmlDocuments();
        });
        builder.Services.AddAssemblyServices(builder.Configuration);

        return builder;
    }

    public static WebApplication ConfigureServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddlewares();
        app.MapControllers();

        return app;
    }

    private static void AddSwaggerXmlDocuments(this SwaggerGenOptions config)
    {
        var applicationXml = Path.Combine(AppContext.BaseDirectory, "Tiny.Application.xml");
        var apiXml = Path.Combine(AppContext.BaseDirectory, "Tiny.Api.xml");
        config.IncludeXmlComments(applicationXml);
        config.IncludeXmlComments(apiXml, true);
    }

    private static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>()
                    .UseMultiTenantMiddleware(CustomRequestHeader.TenantId);
    }

    private static IServiceCollection AddAssemblyServices(this IServiceCollection services, IConfiguration configuration)
    {
        Application.ConfigureServiceContainer.AddServices(services);
        Infrastructure.ConfigureServiceContainer.AddServices(services, configuration);
        ConfigureServiceContainer.AddServices(services);

        return services;
    }
}
