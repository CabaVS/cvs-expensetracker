using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceRepository(ApplicationDbContext dbContext) : IBalanceRepository
{
    public Task<Guid> CreateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEf.FromDomain(balance, workspaceId);
        
        EntityEntry<BalanceEf> added = dbContext.Balances.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }
}
