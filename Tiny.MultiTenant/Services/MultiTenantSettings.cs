// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.MultiTenant.Interfaces;

namespace Tiny.MultiTenant.Services;

internal class MultiTenantSettings : IMultiTenantSettings
{
    public string TenantIdFieldName { get; set; } = string.Empty;
    public bool UseTenantIdField { get; set; }
    public Type MiddlewareType { get; set; } = default!;
    public bool UseDistributedCache { get; set; }


    private static MultiTenantSettings? s_instance;

    public static MultiTenantSettings Instance
    {
        get
        {
            if (s_instance is null)
                s_instance = new MultiTenantSettings();

            return s_instance;
        }
        
    }
    private MultiTenantSettings()
    {
    }
}
