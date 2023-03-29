// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Enums;

namespace Tiny.Api.Conventions;

public class TinyApiConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                var responseTypeByActionAttribute = action.Attributes.OfType<ResponseTypeByActionAttribute>().FirstOrDefault();
                if (responseTypeByActionAttribute == null)
                    continue;
                
                //TODO : responseTypeByAction.Action 에 따른 ProduceResponseTypeAttribute생성
                var actionMethod = GetHttpMethod(action);
                var statusCode = DefaultSuccessCodes.Default[actionMethod];

                var responseTypeByActionAttributeType = responseTypeByActionAttribute.GetType();
                if (responseTypeByActionAttributeType.IsGenericType)
                {
                    var genericTypeArgument = responseTypeByActionAttributeType.GenericTypeArguments[0];
                    action.Filters.Add(new ProducesResponseTypeAttribute(genericTypeArgument, statusCode));
                }
                else
                {
                    action.Filters.Add((new ProducesResponseTypeAttribute(statusCode)));
                }

                var responseTypeAttributes =
                    ResponseObjectGenerator.GenerateBy(responseTypeByActionAttribute.ResponseBy);
                foreach (var responseTypeAttribute in responseTypeAttributes)
                {
                    action.Filters.Add(responseTypeAttribute);
                }
            }
        }
    }

    private HttpActionMethod GetHttpMethod(ActionModel actionModel)
    {
        if (actionModel.Attributes.OfType<HttpGetAttribute>().Any())
            return HttpActionMethod.Get;
        else if (actionModel.Attributes.OfType<HttpPostAttribute>().Any())
            return HttpActionMethod.Post;
        else if (actionModel.Attributes.OfType<HttpPutAttribute>().Any())
            return HttpActionMethod.Put;
        else if (actionModel.Attributes.OfType<HttpDeleteAttribute>().Any())
            return HttpActionMethod.Delete;

        return HttpActionMethod.None;
    }
}
