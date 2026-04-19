using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexoraERP.Application.Accounting.Commands.CreateChartAccount;
using NexoraERP.Application.Accounting.Models;
using NexoraERP.Application.Accounting.Queries.GetChartOfAccountsTree;

namespace NexoraERP.Api.Controllers.V1;

[ApiController]
[Route("api/v1/chart-of-accounts")]
public sealed class ChartOfAccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet("tree")]
    [ProducesResponseType(typeof(IReadOnlyList<ChartAccountNodeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ChartAccountNodeDto>>> GetTree(CancellationToken cancellationToken)
    {
        var tree = await mediator.Send(new GetChartOfAccountsTreeQuery(), cancellationToken);
        return Ok(tree);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateChartAccountCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, id);
    }
}
