using Tiny.Api.ApplicationImplements;
using Tiny.Application.Interfaces;

namespace Tiny.Api;

internal static class ConfigureServiceContainer
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionStore, DbConnectionStore>();
    }
}
