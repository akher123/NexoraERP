using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexoraERP.Application.TenantInfo;
using NexoraERP.Application.TenantInfo.Queries.GetCurrentTenant;

namespace NexoraERP.Api.Controllers;

[ApiController]
[Route("api/v1/tenant-context")]
public sealed class TenantContextController(IMediator mediator) : ControllerBase
{
    [HttpGet("current")]
    public async Task<ActionResult<TenantInfoDto>> GetCurrent(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetCurrentTenantQuery(), cancellationToken);
        if (dto is null)
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Tenant is not resolved.");

        return Ok(dto);
    }
}
