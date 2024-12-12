using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceRepository(ApplicationDbContext dbContext) : IBalanceRepository
{
    public async Task<Guid> Create(Guid workspaceId, Domain.Entities.Balance balance, CancellationToken cancellationToken)
    {
        var balanceToCreate = Balance.ConvertFromDomain(balance, workspaceId);
        
        var addedEntityEntry = await dbContext.Balances.AddAsync(balanceToCreate, cancellationToken);
        
        return addedEntityEntry.Entity.Id;
    }
}