using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Application.Notes;

namespace NexoraERP.Application.Notes.Queries.GetNotesList;

public sealed class GetNotesListQueryHandler(INotesRepository notesRepository)
    : IRequestHandler<GetNotesListQuery, IReadOnlyList<NoteListItemDto>>
{
    public async Task<IReadOnlyList<NoteListItemDto>> Handle(
        GetNotesListQuery request,
        CancellationToken cancellationToken)
    {
        var notes = await notesRepository.ListOrderedByCreatedDescendingAsync(cancellationToken);
        return notes.Select(n => new NoteListItemDto(n.Id, n.Title, n.CreatedAtUtc)).ToList();
    }
}
