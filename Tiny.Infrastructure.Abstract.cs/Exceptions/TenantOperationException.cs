// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Infrastructure.Abstract.Exceptions;

public class TenantOperationException : Exception
{
    public TenantOperationException(string? message) : base(message)
    {
    }

    public TenantOperationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
