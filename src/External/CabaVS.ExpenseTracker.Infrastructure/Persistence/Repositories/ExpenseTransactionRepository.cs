using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainExpenseTransaction = CabaVS.ExpenseTracker.Domain.Entities.ExpenseTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseTransactionRepository(
    ApplicationDbContext dbContext) : IExpenseTransactionRepository
{
    public async Task<Guid> Create(DomainExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseTransaction.FromDomain(expenseTransaction, workspaceId);

        var added = await dbContext.ExpenseTransactions.AddAsync(entity, ct);
        dbContext.ExpenseCategories.Update(entity.Destination);
        dbContext.Balances.Update(entity.Source);

        return added.Entity.Id;
    }
}