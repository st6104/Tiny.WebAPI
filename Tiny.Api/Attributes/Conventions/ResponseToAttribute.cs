// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.ResponseObjects;

namespace Tiny.Api.Attributes.Conventions;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
internal abstract class ResponseToAttributeBase: Attribute
{
    public int StatusCode { get; }

    protected ResponseToAttributeBase(int statusCode)
    {
        StatusCode = statusCode;
    }

    public abstract Type GetResponseType();
}

internal class ResponseToAttribute<TResponse> : ResponseToAttributeBase
{
    public ResponseToAttribute(int statusCode) : base(statusCode)
    {
    }

    public override Type GetResponseType()
    {
        return typeof(TResponse);
    }
}

internal class ResponseToNotFound : ResponseToAttribute<NotFoundObject>
{
    public ResponseToNotFound() : base(StatusCodes.Status404NotFound)
    {
    }
}

internal class ResponseToBadRequest : ResponseToAttribute<BadRequestObject>
{
    public ResponseToBadRequest() : base(StatusCodes.Status400BadRequest)
    {
    }
}
internal class ResponseToForbidden : ResponseToAttribute<ForbiddenObject>
{
    public ResponseToForbidden() : base(StatusCodes.Status403Forbidden)
    {
    }
}

internal class ResponseToUnauthorized : ResponseToAttribute<UnauthorizedObject>
{
    public ResponseToUnauthorized() : base(StatusCodes.Status401Unauthorized)
    {
    }
}

internal class ResponseToServerError : ResponseToAttribute<ServerErrorObject>
{
    public ResponseToServerError() : base(StatusCodes.Status500InternalServerError)
    {
    }
}