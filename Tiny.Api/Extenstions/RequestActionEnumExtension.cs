using System.Reflection;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Enums;

namespace Tiny.Api.Extenstions;

internal static class RequestActionEnumExtension
{
    public static SuccessStatusCodeAttribute? GetSuccessStatusAttribute(this RequestAction requestAction)
    {
        return requestAction.GetFieldInfo().GetCustomAttribute<SuccessStatusCodeAttribute>();
    }

    public static IEnumerable<ResponseToAttributeBase> GetResponseToAttributes(this RequestAction requestAction)
    {
        var attributes = requestAction.GetFieldInfo().GetCustomAttributes<ResponseToAttributeBase>();
        return attributes;
    }

    private static FieldInfo GetFieldInfo(this RequestAction requestAction)
    {
        return requestAction.GetType().GetField(requestAction.ToString())!;
    }
}
