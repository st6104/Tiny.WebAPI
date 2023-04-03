using System.Diagnostics.Metrics;
using Tiny.Infrastructure;
using Tiny.Worker.DomainGenerator;
using Tiny.Worker.DomainGenerator.AppSettingOptions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var dbConnectionOption = hostContext.Configuration.GetSection(DbConnectionOption.SectionName)
            .Get<DbConnectionOption>() ?? new DbConnectionOption();

        services.AddSingleton(dbConnectionOption);

        Tiny.Infrastructure.ConfigureServiceContainer.AddServices(services);
        services.AddScoped<IGenerateGLAccountTask, GenerateGLAccountTask>();
        services.AddHostedService<Worker>();
    })
    .Build();


host.Run();
