using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.MultiTenant;
using Tiny.Shared.Extensions;

namespace Tiny.Worker.DomainGenerator;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var parentScope = _serviceProvider.CreateScope())
        {
            var parentServiceProvider = parentScope.ServiceProvider;
            var multiTenantStore = parentServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
            var tenants = await multiTenantStore.GetAllAsync(stoppingToken);
            try
            {
                await tenants.ForEachAsync(async (tenant, cancellationToken) =>
                {
                    using (var childScope = parentServiceProvider.CreateScope())
                    {
                        var childScopeServiceProvider = childScope.ServiceProvider;
                        var multiTenantService = childScopeServiceProvider.GetRequiredService<IMultiTenantService>();
                        multiTenantService.Current = tenant;

                        var generatorService = childScopeServiceProvider.GetRequiredService<IGenerateGLAccountTask>();
                        await generatorService.ExecuteAsync(cancellationToken);
                    }
                }, stoppingToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
