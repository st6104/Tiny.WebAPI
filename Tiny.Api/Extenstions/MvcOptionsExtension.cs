using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Tiny.Api.Extenstions;

internal static class MvcOptionsExtension
{
    internal static MvcOptions SetResultConvention(this MvcOptions options)
    {
        return options.AddResultConvention(resultStatusMapConfig => resultStatusMapConfig.SetResultStatusMap());
    }

    private static ResultStatusMap SetResultStatusMap(this ResultStatusMap map)
    {
        return map.AddDefaultMap()
                    .ForOkResult();
                    // .ForErrorResult()
                    // .ForNotFoundResult()
                    // .ForInvalidResult();
    }

    private static ResultStatusMap ForOkResult(this ResultStatusMap map)
    {
        return map.For(ResultStatus.Ok,
                        HttpStatusCode.OK,
                        resultStatisOptions => resultStatisOptions.SetOkStatusDetails());
    }

    public static ResultStatusOptions SetOkStatusDetails(this ResultStatusOptions options)
    {
        return options.For("POST", HttpStatusCode.Created)
                        .For("PUT", HttpStatusCode.Created)
                        .For("DELETE", HttpStatusCode.NoContent);
    }

    // private static ResultStatusMap ForErrorResult(this ResultStatusMap map)
    // {
    //     return map.ForWithOriginalResult(ResultStatus.Error, HttpStatusCode.InternalServerError);
    // }

    // private static ResultStatusMap ForNotFoundResult(this ResultStatusMap map)
    // {
    //     return map.ForWithOriginalResult(ResultStatus.NotFound, HttpStatusCode.NotFound);
    // }

    // private static ResultStatusMap ForInvalidResult(this ResultStatusMap map)
    // {
    //     return map.ForWithOriginalResult(ResultStatus.Invalid, HttpStatusCode.BadRequest);
    // }

    // private static ResultStatusMap ForWithOriginalResult(this ResultStatusMap map, ResultStatus status, HttpStatusCode defaultStatusCode)
    // {
    //     return map.For(status, defaultStatusCode, statusOption => statusOption.With((_, result) => result));
    // }
}
