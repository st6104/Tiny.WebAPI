// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.Enums;

namespace Tiny.Api.Attributes.Conventions;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ResponseTypeByActionAttribute : Attribute
{
    public ResponseBy ResponseBy { get; }

    public ResponseTypeByActionAttribute(ResponseBy responseBy)
    {
        ResponseBy = responseBy;
    }
}

public sealed class ResponseTypeByActionAttribute<TSuccess> : ResponseTypeByActionAttribute
{
    public ResponseTypeByActionAttribute(ResponseBy responseBy) : base(responseBy)
    {
    }
}
