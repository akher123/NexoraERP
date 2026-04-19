using MediatR;
using NexoraERP.Application.Accounting.Models;

namespace NexoraERP.Application.Accounting.Queries.GetJournalEntryById;

public sealed record GetJournalEntryByIdQuery(Guid Id) : IRequest<JournalEntryDetailDto?>;
