using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainIncomeTransaction = CabaVS.ExpenseTracker.Domain.Entities.IncomeTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeTransactionRepository(ApplicationDbContext dbContext) : IIncomeTransactionRepository
{
    public async Task<DomainIncomeTransaction?> GetById(Guid incomeTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.IncomeTransactions
            .AsNoTracking()
            .Include(it => it.Source)
            .ThenInclude(ic => ic.Currency)
            .Include(it => it.Destination)
            .ThenInclude(ic => ic.Currency)
            .Where(it => it.Id == incomeTransactionId)
            .Where(it => it.Source.WorkspaceId == workspaceId)
            .Where(it => it.Destination.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(ct);
        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainIncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeTransaction.FromDomain(incomeTransaction, workspaceId);

        var added = await dbContext.IncomeTransactions.AddAsync(entity, ct);
        dbContext.IncomeCategories.Update(entity.Source);
        dbContext.Balances.Update(entity.Destination);

        return added.Entity.Id;
    }

    public Task Update(DomainIncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeTransaction.FromDomain(incomeTransaction, workspaceId);

        _ = dbContext.IncomeTransactions.Update(entity);

        return Task.CompletedTask;
    }

    public Task Delete(DomainIncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeTransaction.FromDomain(incomeTransaction, workspaceId);

        _ = dbContext.IncomeTransactions.Remove(entity);

        return Task.CompletedTask;
    }
}