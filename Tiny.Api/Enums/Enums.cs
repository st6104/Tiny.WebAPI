// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.Attributes.Conventions;

namespace Tiny.Api.Enums;

public enum HttpActionMethod
{
    None,
    Get,
    Post,
    Put,
    Delete
}

public enum RequestAction
{
    [SuccessStatusCode(StatusCodes.Status200OK)]
    [ResponseToNotFound]
    [ResponseToServerError]
    GetOne,

    [SuccessStatusCode(StatusCodes.Status200OK)]
    [ResponseToServerError]
    GetMany,

    [SuccessStatusCode(StatusCodes.Status201Created)]
    [ResponseToBadRequest]
    [ResponseToServerError]
    Post,

    [SuccessStatusCode(StatusCodes.Status204NoContent)]
    [ResponseToNotFound]
    [ResponseToBadRequest]
    [ResponseToServerError]
    Put,

    [SuccessStatusCode(StatusCodes.Status204NoContent)]
    [ResponseToNotFound]
    [ResponseToServerError]
    Delete
}
