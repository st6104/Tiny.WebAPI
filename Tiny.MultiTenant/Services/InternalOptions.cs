// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Caching.Distributed;

namespace Tiny.MultiTenant.Services;

internal static class InternalOptions
{
    private static TimeSpan s_fromSeconds = TimeSpan.FromSeconds(30);
    public static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions =
        new DistributedCacheEntryOptions().SetSlidingExpiration(s_fromSeconds);

    
}
