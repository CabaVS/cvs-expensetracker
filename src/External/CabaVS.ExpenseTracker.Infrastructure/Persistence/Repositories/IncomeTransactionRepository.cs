using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainIncomeTransaction = CabaVS.ExpenseTracker.Domain.Entities.IncomeTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeTransactionRepository(
    ApplicationDbContext dbContext) : IIncomeTransactionRepository
{
    public async Task<Guid> Create(DomainIncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeTransaction.FromDomain(incomeTransaction, workspaceId);

        var added = await dbContext.IncomeTransactions.AddAsync(entity, ct);
        dbContext.IncomeCategories.Update(entity.Source);
        dbContext.Balances.Update(entity.Destination);

        return added.Entity.Id;
    }
}