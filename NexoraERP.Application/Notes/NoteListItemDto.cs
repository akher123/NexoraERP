namespace NexoraERP.Application.Notes;

public sealed record NoteListItemDto(Guid Id, string Title, DateTimeOffset CreatedAtUtc);
