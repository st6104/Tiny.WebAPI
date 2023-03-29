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

    [HttpGet("{id}")]
    [ResponseTypeByAction<GLAccountViewModel>(ResponseBy.GetOne)]
    public async Task<ActionResult> GetOneAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var glAccount = await _mediator.Send(new GLAccountGetOneQuery(id), cancellationToken);
        return Ok(glAccount);
    }

    [HttpGet]
    [ResponseTypeByAction<IReadOnlyList<GLAccountViewModel>>(ResponseBy.GetMany)]
    public async Task<ActionResult> GetManyAsync(CancellationToken cancellationToken)
    {
        var glAccounts = await _mediator.Send(new GLAccountGetManyQuery(), cancellationToken);
        return Ok(glAccounts);
    }

    [HttpPost]
    [ResponseTypeByAction(ResponseBy.Post)]
    public async Task<ActionResult> PostAsync([FromBody] GLAccountAddCommand request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(request, cancellationToken);
        return Created("", id);
    }

    [HttpPut("{id}")]
    [ResponseTypeByAction<GLAccountViewModel>(ResponseBy.Put)]
    public async Task<ActionResult> PutAsync([FromRoute] long id, [FromBody] GLAccountUpdateRequest body, CancellationToken cancellationToken)
    {
        var command = body.ToCommand(id);
        var glAccountViewModel = await _mediator.Send(command, cancellationToken);
        return Created("", glAccountViewModel);
    }
    
    //TODO : Delete 메소드를 만들어서 ResponseTypeByActionAttribute 테스트
}
