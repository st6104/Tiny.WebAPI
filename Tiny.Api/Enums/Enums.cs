// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.Attributes.Conventions;
using Tiny.Api.ResponseObjects;

namespace Tiny.Api.Enums;

public enum HttpActionMethod
{
    None,
    Get,
    Post,
    Put,
    Delete
}

public enum ResponseBy
{
    [ResponseObjectBy<NotFoundObject>(StatusCodes.Status404NotFound)]
    [ResponseObjectBy<ServerErrorObject>(StatusCodes.Status500InternalServerError)]
    GetOne,
    
    [ResponseObjectBy<ServerErrorObject>(StatusCodes.Status500InternalServerError)]
    GetMany,
    
    [ResponseObjectBy<BadRequestObject>(StatusCodes.Status400BadRequest)]
    [ResponseObjectBy<ServerErrorObject>(StatusCodes.Status500InternalServerError)]
    Post,
    
    [ResponseObjectBy<BadRequestObject>(StatusCodes.Status400BadRequest)]
    [ResponseObjectBy<ServerErrorObject>(StatusCodes.Status500InternalServerError)]
    Put,
    
    [ResponseObjectBy<NotFoundObject>(StatusCodes.Status404NotFound)]
    [ResponseObjectBy<ServerErrorObject>(StatusCodes.Status500InternalServerError)]
    Delete
}
