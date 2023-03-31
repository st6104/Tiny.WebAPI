using System.Net;
using FluentValidation;
using Tiny.Api.ResponseObjects;
using Tiny.Infrastructure.Abstract.Exceptions;
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

    private Task SetResponseObjectTo(HttpResponse httpResponse, Exception exception)
    {
        httpResponse.ContentType = ResponseContentTypeToJson;

        Task task;
        
        switch (exception)
        {
            case TenantNotFoundException tenantNotFoundException:
                httpResponse.StatusCode = (int)HttpStatusCode.NotFound;
                task = httpResponse.WriteAsJsonAsync(new NotFoundObject(NotFoundObject.TenantId,
                    tenantNotFoundException.Message));
                break;
            case EntityIdNotFoundException entityIdNotFoundExcption:
                httpResponse.StatusCode = (int)HttpStatusCode.NotFound;
                task = httpResponse.WriteAsJsonAsync(new NotFoundObject(NotFoundObject.EntityId,
                    entityIdNotFoundExcption.Message));
                break;
            case DomainValidationErrorException domainValidationErrorException:
                httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                task = httpResponse.WriteAsJsonAsync(new BadRequestObject(domainValidationErrorException.Identifier,
                    domainValidationErrorException.Message));
                break;
            case ValidationException validationException:
                httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                task = httpResponse.WriteAsJsonAsync(new BadRequestObject(validationException.Errors));
                break;
            default:
                httpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                task = httpResponse.WriteAsJsonAsync(new ServerErrorObject(exception.Message));
                break;
        }

        return task;
    }
}
