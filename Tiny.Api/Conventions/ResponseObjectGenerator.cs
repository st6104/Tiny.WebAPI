// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Enums;
using Tiny.Api.ResponseObjects;

namespace Tiny.Api.Conventions;

public static class ResponseObjectGenerator
{
    private static readonly ProducesResponseTypeAttribute UnauthorizeResponse =
        new ProducesResponseTypeAttribute(typeof(UnauthorizedObject), StatusCodes.Status401Unauthorized);

    private static readonly ProducesResponseTypeAttribute ForbiddenResponse =
        new ProducesResponseTypeAttribute(typeof(ForbiddenObject),StatusCodes.Status403Forbidden);

    public static IReadOnlyList<ProducesResponseTypeAttribute> GenerateBy(ResponseBy responseBy)
    {
        var producesResponseTypeAttributes =
            new List<ProducesResponseTypeAttribute> { UnauthorizeResponse, ForbiddenResponse };

        var producesResponseTypeAttributesPerAction = GetResponseObjectMeta(responseBy).Select(resObj =>
            new ProducesResponseTypeAttribute(resObj.GetResponseType(), resObj.StatusCode));

        return producesResponseTypeAttributes.Concat(producesResponseTypeAttributesPerAction).ToList().AsReadOnly();
    }

    private static IReadOnlyList<ResponseObjectByAttributeBase> GetResponseObjectMeta(ResponseBy responseBy)
    {
        var fieldInfo = responseBy.GetType().GetField(responseBy.ToString());
        return fieldInfo.GetCustomAttributes().OfType<ResponseObjectByAttributeBase>().ToList().AsReadOnly();
    }
}
