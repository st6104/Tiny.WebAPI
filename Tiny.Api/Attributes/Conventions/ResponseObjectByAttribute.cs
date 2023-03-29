// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Tiny.Api.Attributes.Conventions;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
internal abstract class ResponseObjectByAttributeBase: Attribute
{
    public int StatusCode { get; }

    public ResponseObjectByAttributeBase(int statusCode)
    {
        StatusCode = statusCode;
    }

    public abstract Type GetResponseType();
}

internal sealed class ResponseObjectByAttribute<TResponse> : ResponseObjectByAttributeBase 
{
    public ResponseObjectByAttribute(int statusCode) : base(statusCode)
    {
    }

    public override Type GetResponseType()
    {
        return typeof(TResponse);
    }
}
