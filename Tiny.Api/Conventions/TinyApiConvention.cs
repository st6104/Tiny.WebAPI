// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Extenstions;

namespace Tiny.Api.Conventions;

public class TinyApiConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                var produceResponseTypes = action.Attributes.OfType<ProducesResponseTypeForAttribute>().FirstOrDefault();
                if (produceResponseTypes == null)
                    continue;

                // var httpMethod = GetHttpMethod(action);
                var successStatusCode = produceResponseTypes.GetSuccessStatusCode();

                var producesSuccessResponseTypeAttribute = new ProducesResponseTypeAttribute(successStatusCode);
                if (produceResponseTypes.TryGetGenericArgument(out var successObjectType))
                    producesSuccessResponseTypeAttribute.Type = successObjectType!;

                action.Filters.Add(producesSuccessResponseTypeAttribute);

                foreach (var errorResponseTypeAttribute in produceResponseTypes.GetProduceResponseTypeAttributes())
                {
                    action.Filters.Add(errorResponseTypeAttribute);
                }
            }
        }
    }

    // private static HttpActionMethod GetHttpMethod(ActionModel actionModel)
    // {
    //     if (actionModel.Attributes.OfType<HttpGetAttribute>().Any())
    //         return HttpActionMethod.Get;
    //     else if (actionModel.Attributes.OfType<HttpPostAttribute>().Any())
    //         return HttpActionMethod.Post;
    //     else if (actionModel.Attributes.OfType<HttpPutAttribute>().Any())
    //         return HttpActionMethod.Put;
    //     else if (actionModel.Attributes.OfType<HttpDeleteAttribute>().Any())
    //         return HttpActionMethod.Delete;
    //
    //     return HttpActionMethod.None;
    // }
}
