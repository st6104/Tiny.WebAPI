using FluentValidation;
using Tiny.Api.Extenstions;
using Tiny.MultiTenant.Exceptions;
using Tiny.Shared.Exceptions;

namespace Tiny.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private const string ResponseContentTypeToJson = "appliction/json";

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await SetResponseObjectTo(context.Response, ex);
        }
    }

    private static Task SetResponseObjectTo(HttpResponse httpResponse, Exception exception)
    {
        httpResponse.ContentType = ResponseContentTypeToJson;

        var task = exception switch
        {
            TenantNotFoundException tenantNotFoundException => httpResponse.AssignResponseAsTenantNotFound(tenantNotFoundException),
            EntityIdNotFoundException entityIdNotFoundExcption => httpResponse.AssignResponseAsEntityNotFound(entityIdNotFoundExcption),
            DomainValidationErrorException domainValidationErrorException => httpResponse.AssignResponseAsDomainInvalid(domainValidationErrorException),
            ValidationException validationException => httpResponse.AssignResponseAsInvalid(validationException),
            _ => httpResponse.AssignResponseAsServerError(exception)
        };

        return task;
    }
}
