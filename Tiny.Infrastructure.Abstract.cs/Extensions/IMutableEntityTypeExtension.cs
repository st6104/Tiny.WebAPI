// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Tiny.Infrastructure.Abstract.Extensions;

internal static class IMutableEntityTypeExtension
{
    public static bool IsExistProperty(this IMutableEntityType mutableEntityType, string propertyName, bool ifExistsWhenThrowException = false)
    {
        var property = mutableEntityType.FindProperty(propertyName);

        if (ifExistsWhenThrowException && property != null)
            throw new ArgumentException(
                $"[{mutableEntityType.ClrType}] : [{propertyName}] property is already exists.");

        return property is not null;
    }
}
