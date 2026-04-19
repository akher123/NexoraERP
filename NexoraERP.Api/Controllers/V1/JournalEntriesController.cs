using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexoraERP.Application.Accounting.Commands.PostJournalEntry;
using NexoraERP.Application.Accounting.Models;
using NexoraERP.Application.Accounting.Queries.GetJournalEntryById;
using NexoraERP.Application.Accounting.Queries.ListJournalEntries;

namespace NexoraERP.Api.Controllers.V1;

[ApiController]
[Route("api/v1/journal-entries")]
public sealed class JournalEntriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedJournalEntriesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedJournalEntriesResponse>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var (items, total) = await mediator.Send(new ListJournalEntriesQuery(page, pageSize), cancellationToken);
        return Ok(new PagedJournalEntriesResponse(items, total, page, pageSize));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JournalEntryDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JournalEntryDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetJournalEntryByIdQuery(id), cancellationToken);
        if (dto is null)
            return NotFound();
        return Ok(dto);
    }

    /// <summary>Validates double-entry in base currency and persists a posted journal.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> PostJournal(
        [FromBody] PostJournalEntryCommand command,
        CancellationToken cancellationToken)
    {
        var journalId = await mediator.Send(command, cancellationToken);
        return Ok(journalId);
    }
}

public sealed record PagedJournalEntriesResponse(
    IReadOnlyList<JournalEntrySummary> Items,
    int TotalCount,
    int Page,
    int PageSize);
