using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiny.Api.Attributes.Conventions;
using Tiny.Api.Enums;
using Tiny.Api.RequestObjects;
using Tiny.Application.Handlers.Commands;
using Tiny.Application.Handlers.Queries;
using Tiny.Application.ViewModels;

namespace Tiny.Api.Controllers;

/// <summary>
/// 계정과목
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GLAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public GLAccountController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseTypeFor<GLAccountViewModel>(RequestAction.GetOne)]
    public async Task<ActionResult> GetOneAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var glAccount = await _mediator.Send(new GLAccountGetOneQuery(id), cancellationToken);
        return Ok(glAccount);
    }

    [HttpGet]
    [ProducesResponseTypeFor<IReadOnlyList<GLAccountViewModel>>(RequestAction.GetMany)]
    public async Task<ActionResult> GetManyTopNAsync(int queryCount, int skipCount, CancellationToken cancellationToken)
    {
        var glAccounts = await _mediator.Send(new GLAccountGetTopNQuery(queryCount, skipCount), cancellationToken);
        return Ok(glAccounts);
    }

    [HttpGet("All")]
    [ProducesResponseTypeFor<IReadOnlyList<GLAccountViewModel>>(RequestAction.GetMany)]
    public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var glAccounts = await _mediator.Send(new GLAccountGetAllQuery(), cancellationToken);
        return Ok(glAccounts);
    }

    [HttpPost]
    [ProducesResponseTypeFor(RequestAction.Post)]
    public async Task<ActionResult> PostAsync([FromBody] GLAccountAddCommand request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(request, cancellationToken);
        return Created("", id);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseTypeFor<GLAccountViewModel>(RequestAction.Put)]
    public async Task<ActionResult> PutAsync([FromRoute] long id, [FromBody] GLAccountUpdateRequest body, CancellationToken cancellationToken)
    {
        var command = body.ToCommand(id);
        var glAccountViewModel = await _mediator.Send(command, cancellationToken);
        return Created("", glAccountViewModel);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseTypeFor(RequestAction.Delete)]
    public Task<ActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        return Task.Run(() => (ActionResult)NoContent(), cancellationToken);
    }
}
