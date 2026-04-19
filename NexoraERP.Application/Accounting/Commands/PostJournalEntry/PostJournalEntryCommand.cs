using MediatR;

namespace NexoraERP.Application.Accounting.Commands.PostJournalEntry;

/// <summary>Creates and posts a balanced journal in one unit of work (tenant base currency validation).</summary>
public sealed record PostJournalEntryCommand(
    DateOnly EntryDate,
    string BaseCurrencyCode,
    string? Reference,
    string? Memo,
    List<PostJournalLineDto> Lines) : IRequest<Guid>;
