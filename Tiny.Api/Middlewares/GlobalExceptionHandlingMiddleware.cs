using System.Net;
using System.Text.Json;
using Ardalis.Result;
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
        catch (TenantNotFoundException tenantNotFoundException)
        {
            context.Response.ContentType = ResponseContentTypeToJson;
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(tenantNotFoundException.Message);
        }
        catch (EntityIdNotFoundException entityIdNotFoundExcption)
        {//TODO: 예외별 응답객체 생성 로직 작성(EntityIdNotFoundException)

        }
        catch (DomainValidationErrorException validationErrorException)
        {//TODO: 예외별 응답객체 생성 로직 작성(DomainValidationErrorException)
            
        }
        catch (Exception ex)
        {
            context.Response.ContentType = ResponseContentTypeToJson;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = Result.Error(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
