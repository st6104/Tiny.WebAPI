// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Tiny.MultiTenant.Extensions;

internal static class IDistributedCacheExtension
{
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key,
        CancellationToken cancellationToken = default) where T : class
    {
        var cachedData = await cache.GetStringAsync(key, cancellationToken);
        return cachedData == null ? default : JsonSerializer.Deserialize<T>(cachedData);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value,
        DistributedCacheEntryOptions? entryOptions = default,
        CancellationToken cancellationToken = default) where T : class
    {
        var cachingData = JsonSerializer.Serialize(value);
        if (string.IsNullOrWhiteSpace(cachingData))
            throw new ArgumentNullException(nameof(value), "Serialized Data is empty.");

        if (entryOptions != null)
            await cache.SetStringAsync(key, cachingData, entryOptions, cancellationToken);
        else
            await cache.SetStringAsync(key, cachingData, cancellationToken);
    }

    // public static Task RemoveAsync(this IDistributedCache cache, string key,
    //     CancellationToken cancellationToken = default)
    // {
    //     return cache.RemoveAsync(key, cancellationToken);
    // }
}
