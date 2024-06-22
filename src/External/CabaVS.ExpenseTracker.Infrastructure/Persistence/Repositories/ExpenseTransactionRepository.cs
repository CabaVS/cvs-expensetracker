using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainExpenseTransaction = CabaVS.ExpenseTracker.Domain.Entities.ExpenseTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseTransactionRepository(ApplicationDbContext dbContext) : IExpenseTransactionRepository
{
    public async Task<DomainExpenseTransaction?> GetById(Guid expenseTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.ExpenseTransactions
            .Include(et => et.Source)
            .ThenInclude(b => b.Currency)
            .Include(et => et.Destination)
            .ThenInclude(ec => ec.Currency)
            .Where(et => et.Id == expenseTransactionId)
            .Where(et => et.Source.WorkspaceId == workspaceId)
            .Where(et => et.Destination.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(ct);

        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseTransaction.FromDomain(expenseTransaction, workspaceId);

        var added = await dbContext.ExpenseTransactions.AddAsync(entity, ct);
        dbContext.ExpenseCategories.Update(entity.Destination);
        dbContext.Balances.Update(entity.Source);

        return added.Entity.Id;
    }

    public Task Update(DomainExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseTransaction.FromDomain(expenseTransaction, workspaceId);

        _ = dbContext.ExpenseTransactions.Update(entity);

        return Task.CompletedTask;
    }

    public Task Delete(DomainExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseTransaction.FromDomain(expenseTransaction, workspaceId);

        _ = dbContext.ExpenseTransactions.Remove(entity);

        return Task.CompletedTask;
    }
}