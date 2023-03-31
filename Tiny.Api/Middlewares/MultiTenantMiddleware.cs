using Tiny.Infrastructure.Abstract.Exceptions;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.MultiTenant;

namespace Tiny.Api.Middlewares;

public class MultiTenantMiddleware
{
    private readonly RequestDelegate _next;

    public MultiTenantMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IMultiTenantStore<TenantInfo> tenantStore, IMultiTenantService multiTenantService)
    {
        var tenantId = httpContext.Request.Headers[CustomRequestHeader.TenantId].FirstOrDefault() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(tenantId) && await tenantStore.TryGetByIdAsync(tenantId, out var tenantInfo))
        {
            multiTenantService.Current = tenantInfo;
            await _next(httpContext);
        }
        else
        {
            throw new TenantNotFoundException(tenantId, $"Can not found Tenant Id({tenantId}).");
        }
    }
}
