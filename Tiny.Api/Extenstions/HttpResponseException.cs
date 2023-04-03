// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using FluentValidation;
using Tiny.Api.ResponseObjects;
using Tiny.Infrastructure.Abstract.Exceptions;
using Tiny.Shared.Exceptions;

namespace Tiny.Api.Extenstions;

internal static class HttpResponseException
{
    public static Task AssignResponseAsTenantNotFound(this HttpResponse httpResponse, TenantNotFoundException exception)
    {
        var responseObject = new NotFoundObject(NotFoundObject.TenantId,
            exception.Message);
        return httpResponse.AssignResponseObject(HttpStatusCode.NotFound, responseObject);
    }

    public static Task AssignResponseAsEntityNotFound(this HttpResponse httpResponse,
        EntityIdNotFoundException exception)
    {
        var responseObject = new NotFoundObject(NotFoundObject.EntityId,
            exception.Message);
        return httpResponse.AssignResponseObject(HttpStatusCode.NotFound, responseObject);
    }
    
    public static Task AssignResponseAsDomainInvalid(this HttpResponse httpResponse,
        DomainValidationErrorException exception)
    {
        var responseObject = new BadRequestObject(exception.Identifier, exception.Message);
        return httpResponse.AssignResponseObject(HttpStatusCode.BadRequest, responseObject);
    }

    public static Task AssignResponseAsInvalid(this HttpResponse httpResponse,
        ValidationException exception)
    {
        var responseObject = new BadRequestObject(exception.Errors);
        return httpResponse.AssignResponseObject(HttpStatusCode.BadRequest, responseObject);
    }

    public static Task AssignResponseAsServerError(this HttpResponse httpResponse,
        Exception exception)
    {
        var responseObject = new ServerErrorObject(exception.Message);
        return httpResponse.AssignResponseObject(HttpStatusCode.InternalServerError, responseObject);
    }

    internal static Task AssignResponseObject<TResponse>(this HttpResponse httpResponse, HttpStatusCode statusCode,
        TResponse value, CancellationToken cancellationToken = default)
    {
        httpResponse.StatusCode = (int)statusCode;
        return httpResponse.WriteAsJsonAsync(value, cancellationToken);
    }
}
