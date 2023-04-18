// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Infrastructure.ConfigurationOptions;

public class RedisCacheConfigOptions
{
    public const string SectionName = "RedisCache";
    public string Configurtion { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
}
