// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.Enums;

namespace Tiny.Api.Attributes.Conventions;

[AttributeUsage(AttributeTargets.Method)]
public class ProducesResponseTypeForAttribute : Attribute
{
    public RequestAction RequestAction { get; }

    public int? SuccessStatusCode { get; set; }

    public ProducesResponseTypeForAttribute(RequestAction requestAction)
    {
        RequestAction = requestAction;
    }
}

public sealed class ProducesResponseTypeForAttribute<TSuccess> : ProducesResponseTypeForAttribute
{
    public ProducesResponseTypeForAttribute(RequestAction requestAction) : base(requestAction)
    {
    }
}
