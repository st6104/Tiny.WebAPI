// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Worker.DomainGenerator.AppSettingOptions;

internal class DbConnectionOption
{
    public const string SectionName = "ConnectionStrings";

    public string Default { get; init; } = string.Empty;
}
