using MediatR;
using NexoraERP.Application.Abstractions.Persistence;
using NexoraERP.Domain.Accounting;
using NexoraERP.Domain.Accounting.Exceptions;

namespace NexoraERP.Application.Accounting.Commands.CreateChartAccount;

public sealed class CreateChartAccountCommandHandler(IChartAccountRepository chartAccounts)
    : IRequestHandler<CreateChartAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateChartAccountCommand command, CancellationToken cancellationToken)
    {
        if (await chartAccounts.CodeExistsAsync(command.Code, cancellationToken))
            throw new AccountingRuleViolationException($"Account code '{command.Code}' already exists.");

        ChartAccount account;
        if (command.ParentAccountId is null)
        {
            account = ChartAccount.CreateRoot(
                command.Code,
                command.Name,
                command.AccountType,
                command.NormalBalance,
                command.IsPostingAccount,
                command.SortOrder);
        }
        else
        {
            var parent = await chartAccounts.GetByIdAsync(command.ParentAccountId.Value, cancellationToken);
            if (parent is null)
                throw new AccountingRuleViolationException("Parent account was not found.");

            account = ChartAccount.CreateChild(
                parent,
                command.Code,
                command.Name,
                command.AccountType,
                command.NormalBalance,
                command.IsPostingAccount,
                command.SortOrder);
        }

        await chartAccounts.AddAsync(account, cancellationToken);
        return account.Id;
    }
}
