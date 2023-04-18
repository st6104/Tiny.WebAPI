// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.MultiTenant.Exceptions;

public sealed class TenantNotFoundException : Exception
{
    private readonly bool _isAssignedMessage;
    public string TenantId { get; }

    public override string Message => _isAssignedMessage ? base.Message : $"Can not found Tenant Id({TenantId})."; 
    
    public TenantNotFoundException(string tenantId, string? message = null, Exception? innerException = null) : base(
        message, innerException)
    {
        TenantId = tenantId;
        _isAssignedMessage = !string.IsNullOrWhiteSpace(message);
    }

    
}
