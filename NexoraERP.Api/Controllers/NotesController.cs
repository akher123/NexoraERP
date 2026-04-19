using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexoraERP.Application.Notes;
using NexoraERP.Application.Notes.Queries.GetNotesList;

namespace NexoraERP.Api.Controllers;

[ApiController]
[Route("api/v1/notes")]
public sealed class NotesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NoteListItemDto>>> List(CancellationToken cancellationToken)
    {
        var items = await mediator.Send(new GetNotesListQuery(), cancellationToken);
        return Ok(items);
    }
}
