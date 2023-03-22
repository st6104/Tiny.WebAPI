using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiny.Api.ActionFilters;
using Tiny.Api.RequestObjects;
using Tiny.Application.Handlers.Commands;
using Tiny.Application.Handlers.Queries;
using Tiny.Application.ViewModels;

namespace Tiny.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GLAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public GLAccountController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("{id}")]
    [ExceptionToResult]
    [ExpectedFailures(ResultStatus.Forbidden, ResultStatus.NotFound)]
    public async Task<Result<GLAccountViewModel>> GetOneAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var glAccount = await _mediator.Send(new GLAccountGetOneQuery(id), cancellationToken);
        return Result.Success(glAccount);
    }

    [HttpGet]
    [ExceptionToResult]
    [ExpectedFailures(ResultStatus.Forbidden)]
    public async Task<Result<IReadOnlyList<GLAccountViewModel>>> GetManyAsync(CancellationToken cancellationToken)
    {
        var glAccounts = await _mediator.Send(new GLAccountGetManyQuery(), cancellationToken);
        return Result.Success(glAccounts);
    }

    [HttpPost]
    [ExceptionToResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<long>> PostAsync([FromBody] GLAccountAddCommand request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(request, cancellationToken);
        return Result.Success(id);
    }

    [HttpPut("{id}")]
    [ExceptionToResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<GLAccountViewModel>> PutAsync([FromRoute] long id, [FromBody] GLAccountUpdateRequest body, CancellationToken cancellationToken)
    {
        var command = body.ToCommand(id);
        var glAccountViewModel = await _mediator.Send(command, cancellationToken);
        return Result.Success(glAccountViewModel);
    }
}
