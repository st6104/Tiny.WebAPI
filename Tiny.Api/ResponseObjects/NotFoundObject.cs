// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Tiny.Api.ResponseObjects;

public class NotFoundObject
{
    public const string EntityId = "EntityId";
    public const string TenantId = "TenantId";
    
    //TODO : 좀 더 명확하게 NotFound Response를 구분할 방법?
    /// <summary>
    /// 에러 유형(EntityId, TenantId)
    /// </summary>
    public string Type { get; }

    public string Message { get; }

    public NotFoundObject(string type, string message)
    {
        Type = type;
        Message = message;
    }
}
