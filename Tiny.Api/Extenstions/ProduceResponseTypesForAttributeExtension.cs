using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Enums;

namespace Tiny.Api.Extenstions;

internal static class ProduceResponseTypesForAttributeExtension
{
    public static int GetSuccessStatusCode(this ProducesResponseTypeForAttribute attr)
    {
        if (attr.SuccessStatusCode.HasValue)
            return attr.SuccessStatusCode.Value;

        var successCodeAttr = attr.RequestAction.GetSuccessStatusAttribute();
        return successCodeAttr?.StatusCode ?? 0;
    }

    public static IEnumerable<ProducesResponseTypeAttribute> GetProduceResponseTypeAttributes(this ProducesResponseTypeForAttribute attr)
    {
        return attr.RequestAction.GetResponseToAttributes()
                                .Select(responseToAttr =>
                                            new ProducesResponseTypeAttribute(responseToAttr.GetResponseType(), responseToAttr.StatusCode));
    }

    public static bool TryGetGenericArgument(this ProducesResponseTypeForAttribute attr, out Type? genericArgument)
    {
        genericArgument = default;
        var attrType = attr.GetType();
        if (attrType.IsGenericType)
        {
            genericArgument = attrType.GenericTypeArguments[0];
            return true;
        }
        else
        {
            return false;
        }
    }
}
