﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.MultiTenant.Interfaces;

public interface IMultiTenantSettings
{
    string TenantIdFieldName { get; }
    
    bool UseTenantIdField { get; }
    
    Type MiddlewareType { get; }
    
    bool UseDistributedCache { get; }
}