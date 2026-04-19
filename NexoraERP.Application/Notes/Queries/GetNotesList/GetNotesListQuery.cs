using MediatR;
using NexoraERP.Application.Notes;

namespace NexoraERP.Application.Notes.Queries.GetNotesList;

public sealed record GetNotesListQuery : IRequest<IReadOnlyList<NoteListItemDto>>;
