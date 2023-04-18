// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.MultiTenant.Interfaces;

namespace Tiny.Infrastructure.MultiTenant;

public class TenantInfo : ITenantInfo
{
    public TenantInfo(string id, string name, string connectionString, bool isActive)
    {
        Id = id;
        Name = name;
        ConnectionString = connectionString;
        IsActive = isActive;
    }

    public string Id { get; private set; }

    public string Name { get; private set; }

    public string ConnectionString { get; private set; }

    public bool IsActive { get; private set; }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangeConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void Active()
    {
        IsActive = true;
    }

    public void Inactive()
    {
        IsActive = false;
    }
}
