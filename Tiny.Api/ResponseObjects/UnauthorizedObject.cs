// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Api.ResponseObjects;

public class UnauthorizedObject
{
    public string Message { get; }

    public UnauthorizedObject(string message)
    {
        this.Message = message;
    }
}
