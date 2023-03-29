// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Infrastructure.Abstract.Exceptions;

public sealed class TenantNotFoundException : Exception
{
    public TenantNotFoundException(string tenantId, string? message = null, Exception? innerException = null) : base(
        message, innerException)
    {
        TenantId = tenantId;
    }

    public string TenantId { get; }
}
