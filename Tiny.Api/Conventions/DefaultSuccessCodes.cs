// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Tiny.Api.Enums;

namespace Tiny.Api.Conventions;

public class DefaultSuccessCodes
{//TODO : mapper를 커스터마이징 가능하게..?
    public static readonly DefaultSuccessCodes Default = new();

    private readonly Dictionary<HttpActionMethod, int> _statusDictionary = new();
    
    public int this [HttpActionMethod method]
    {
        get
        {
            _statusDictionary.TryGetValue(method, out var statusCode);
            return statusCode;
        }
    }

    private DefaultSuccessCodes()
    {
        _statusDictionary.Add(HttpActionMethod.Get, StatusCodes.Status200OK);
        _statusDictionary.Add(HttpActionMethod.Post, StatusCodes.Status201Created);
        _statusDictionary.Add(HttpActionMethod.Put, StatusCodes.Status201Created);
        _statusDictionary.Add(HttpActionMethod.Delete, StatusCodes.Status204NoContent);
    }
}
