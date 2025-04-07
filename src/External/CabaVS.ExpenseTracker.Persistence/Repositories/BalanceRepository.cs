using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceRepository(ApplicationDbContext dbContext) : IBalanceRepository
{
    public async Task<Balance?> GetByIdAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken = default)
    {
        Balance? balance = await dbContext.Balances
            .Where(x => x.Id == balanceId)
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(BalanceEf.ProjectToDomain)
            .FirstOrDefaultAsync(cancellationToken);
        return balance;
    }

    public Task<Guid> CreateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEf.FromDomain(balance, workspaceId);
        
        EntityEntry<BalanceEf> added = dbContext.Balances.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }

    public Task UpdateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEf.FromDomain(balance, workspaceId);
        
        _ = dbContext.Balances.Update(entity);

        return Task.CompletedTask;
    }
}
