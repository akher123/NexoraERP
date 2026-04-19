using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexoraERP.Application.System.Tenants;

namespace NexoraERP.Api.Controllers.V1;

/// <summary>Master-catalog operations (no tenant DB). Secure with policy / gateway in production.</summary>
[ApiController]
[Route("api/v1/system/tenants")]
public sealed class SystemTenantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TenantListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TenantListItemDto>>> List(CancellationToken cancellationToken)
    {
        var items = await mediator.Send(new ListTenantsQuery(), cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Register(
        [FromBody] RegisterTenantCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(List), new { id }, id);
    }
}
