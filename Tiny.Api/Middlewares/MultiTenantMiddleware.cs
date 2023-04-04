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

    public async Task InvokeAsync(HttpContext httpContext, IMultiTenantStore<TenantInfo> tenantStore,
        IMultiTenantService multiTenantService)
    {
        var tenantId = httpContext.Request.Headers[CustomRequestHeader.TenantId].FirstOrDefault() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            //TODO : TenantId 헤더 정보가 없을때 오류 작성(Exception 신규 추가 http상태코드는 400) ex:기존 유효성검증과 상태코드가 겹치는 문제를 고민..
        }

        var tenantInfo = await tenantStore.TryGetByIdAsync(tenantId) ?? throw new TenantNotFoundException(tenantId);
        if (!tenantInfo.IsActive)
        {
            //TODO : Tenant가 Inactive 일 때 오류작성 
        }

        multiTenantService.Current = tenantInfo!;
        await _next(httpContext);
    }
}
