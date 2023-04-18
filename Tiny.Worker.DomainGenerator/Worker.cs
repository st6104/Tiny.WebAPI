using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.MultiTenant;
using Tiny.MultiTenant.Interfaces;
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
        await GenerateGLAccounts("1000", 200_000_000, _serviceProvider, stoppingToken);

        // var tenantId = "1000";
        // await SelectGLAccounts(tenantId, parentServiceProvider, multiTenantStore, stoppingToken);
    }

    // private static async Task SelectGLAccounts(string tenantId,
    //     IServiceProvider parentServiceProvider,
    //     IMultiTenantStore<TenantInfo> tenantStore, CancellationToken stoppingToken)
    // {
    //     var tenant = await tenantStore.TryGetByIdAsync(tenantId, stoppingToken);
    //     var loopCount = 100;
    //
    //     if (tenant == null) return;
    //
    //     using (var scope = parentServiceProvider.CreateScope())
    //     {
    //         var serviceProvider = scope.ServiceProvider;
    //         var multiTenantService = serviceProvider.GetRequiredService<IMultiTenantService>();
    //         multiTenantService.Current = tenant;
    //
    //         var service = serviceProvider.GetRequiredService<LargeSelectForGLAccount>();
    //
    //         for (var i = 0; i < loopCount; i++)
    //         {
    //             await service.ExecuteAsync(stoppingToken);
    //         }
    //     }
    // }

    private  async Task GenerateGLAccounts(string tenantId, int generateCount,
        IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var generatorService = scope.ServiceProvider.GetRequiredService<IGenerateGLAccountTask>();
            await generatorService.ExecuteAsync(tenantId, generateCount, stoppingToken);
        }
    }
}
